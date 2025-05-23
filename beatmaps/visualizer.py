import pygame
import json
import time
import sys
import os

# run visualizer by installing pygame and running
# python3 visualizer.py [JSON FILE] [MP3 FILE]

# window parameters 
SCREEN_WIDTH = 500
SCREEN_HEIGHT = 200
CIRCLE_RADIUS = 40
TAP_LINGER_MS = 150  
CIRCLE_Y = SCREEN_HEIGHT // 2

if len(sys.argv) != 3:
    print("Usage: python3 visualizer.py <beatmap_json> <audio_file>")
    sys.exit(1)

# parse arguments
json_path = sys.argv[1]
audio_path = sys.argv[2]

# validate input files
if not os.path.isfile(json_path):
    print(f"Error: Beatmap file '{json_path}' not found.")
    sys.exit(1)

if not os.path.isfile(audio_path):
    print(f"Error: Audio file '{audio_path}' not found.")
    sys.exit(1)

# load from json file
with open(json_path, 'r') as f:
    beatmap_data = json.load(f)

beatmap = beatmap_data["four_rail"]

#initializing pygame
pygame.init()
screen = pygame.display.set_mode((SCREEN_WIDTH, SCREEN_HEIGHT))
pygame.display.set_caption("Beatmap Visualizer")

pygame.mixer.init()
pygame.mixer.music.load(audio_path)

# fixed positions for each circle
circle_positions = [
    (100, CIRCLE_Y),
    (200, CIRCLE_Y),
    (300, CIRCLE_Y),
    (400, CIRCLE_Y)
]

# keep track of visible notes
active_notes = []
note_index = 0
start_time = None
running = True

# get song playback in ms
def get_song_time():
    return (time.time() - start_time) * 1000  

# main loop
pygame.mixer.music.play()
start_time = time.time()

while running:
    screen.fill((0, 0, 0))

    for event in pygame.event.get():
        if event.type == pygame.QUIT:
            running = False

    song_time = get_song_time()

    # Activate notes
    while note_index < len(beatmap):
        note = beatmap[note_index]
        column = note["column"] - 1  

        # handle tap notes
        if note["type"] == "tap":
            if song_time >= note["time"]:
                active_notes.append({
                    "column": column,
                    "endTime": note["time"] + TAP_LINGER_MS
                })
                note_index += 1
            else:
                break
        
        # handle hold notes
        elif note["type"] == "hold":
            if song_time >= note["startTime"]:
                active_notes.append({
                    "column": column,
                    "endTime": note["endTime"]
                })
                note_index += 1
            else:
                break
        else:
            note_index += 1

    # Draw circles, red if active
    current_time = get_song_time()
    for i, (x, y) in enumerate(circle_positions):
        # check if note duration expired
        is_red = any(note["column"] == i and current_time <= note["endTime"] for note in active_notes)
        color = (255, 0, 0) if is_red else (255, 255, 255)
        pygame.draw.circle(screen, color, (x, y), CIRCLE_RADIUS)

    # Remove expired notes
    active_notes = [n for n in active_notes if current_time <= n["endTime"]]

    # update display
    pygame.display.flip()
    pygame.time.delay(16)

pygame.quit()
