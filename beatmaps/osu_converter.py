import json
import sys
import os

# convert .osu files by running
# python3 osu_converter.py [FILE.OSU]

OSU_WIDTH = 512

# divide osu field into 3/4 columns 
def get_column(x, lanes):
    column_width = OSU_WIDTH / lanes
    return int(min(x, OSU_WIDTH - 1) / column_width) + 1

# parse timing section to get beat length in ms per beat
def parse_timing_points(lines):
    for i, line in enumerate(lines):
        if line.strip() == "[TimingPoints]":
            break
    for line in lines[i+1:]:
        if not line.strip() or line.startswith("["):
            break
        parts = line.strip().split(',')
        if len(parts) > 1:
            beat_length = float(parts[1])
            return beat_length  # ms per beat
    return 500.0  # fallback

# retrieve slider speed multiplier from difficulty section
def parse_slider_multiplier(lines):
    for i, line in enumerate(lines):
        if line.strip() == "[Difficulty]":
            break
    for line in lines[i+1:]:
        if not line.strip() or line.startswith("["):
            break
        if line.startswith("SliderMultiplier"):
            return float(line.split(":")[1].strip())
    return 1.4  # fallback

# convert slider length to duration in ms
def calculate_slider_duration(slider_length, beat_length, slider_multiplier):
    return (slider_length * beat_length) / (slider_multiplier * 100)

# parse single beat hit objects
def parse_hit_object(line, beat_length, slider_multiplier):
    parts = line.split(',')

    x = int(parts[0])           # x position of note
    time = int(parts[2])        # timestamp in ms
    type_flags = int(parts[3])  # flag to indicate type of hit obj
    object_data = {"x": x}     

    is_slider = type_flags & 0b10
    is_spinner = type_flags & 0b1000

    if is_slider:
        slider_length = float(parts[7])  # pixel length of slider
        duration = calculate_slider_duration(slider_length, beat_length, slider_multiplier)
        object_data["type"] = "hold"
        object_data["startTime"] = time
        object_data["endTime"] = int(time + duration)
    elif is_spinner:
        end_time = int(parts[5])
        object_data["type"] = "hold"
        object_data["startTime"] = time
        object_data["endTime"] = end_time
    else:
        object_data["type"] = "tap"
        object_data["time"] = time

    return object_data

# convert .osu to .JSON
def convert_osu_file(osu_file_path):
    with open(osu_file_path, 'r', encoding='utf-8') as f:
        lines = f.readlines()

    beat_length = parse_timing_points(lines)
    slider_multiplier = parse_slider_multiplier(lines)

    hit_objects = []
    in_hit_objects = False

    # collect HitObjects 
    for line in lines:
        line = line.strip()
        if line == "[HitObjects]":
            in_hit_objects = True
            continue
        if in_hit_objects:
            if line == "" or line.startswith("//"):
                continue
            note = parse_hit_object(line, beat_length, slider_multiplier)
            hit_objects.append(note)

    notes_3rail = []
    notes_4rail = []

    for note in hit_objects:
        x = note["x"]
        note_3 = note.copy()
        note_4 = note.copy()

        note_3["column"] = get_column(x, 3)
        note_4["column"] = get_column(x, 4)

        note_3.pop("x")
        note_4.pop("x")

        notes_3rail.append(note_3)
        notes_4rail.append(note_4)

    # final output structure
    result = {
        "three_rail": notes_3rail,
        "four_rail": notes_4rail
    }

    # save to JSON
    base_name = os.path.splitext(os.path.basename(osu_file_path))[0]
    output_path = f"{base_name}_converted.json"
    with open(output_path, "w") as out_file:
        json.dump(result, out_file, indent=2)

    print(f"Beatmap converted and saved to: {output_path}")

# entry point
if __name__ == "__main__":
    if len(sys.argv) != 2:
        print("Usage: python osu_converter.py your_map.osu")
        sys.exit(1)

    osu_file = sys.argv[1]
    if not os.path.isfile(osu_file):
        print(f"File not found: {osu_file}")
        sys.exit(1)

    convert_osu_file(osu_file)
