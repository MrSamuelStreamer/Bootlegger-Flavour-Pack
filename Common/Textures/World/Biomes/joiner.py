import os
import random
from PIL import Image

width = 3
height = 3

def create_city_composite():
    # Get all City tiles in the current directory
    current_dir = os.path.dirname(os.path.abspath(__file__))
    city_tiles = []
    
    # Find all MSSBL_City{i}.png files
    for file in os.listdir(current_dir):
        if file.startswith("MSSBL_City") and file.endswith(".png") and file != "MSSBL_City.png":
            city_tiles.append(file)
    
    if not city_tiles:
        print("No MSSBL_City{i}.png files found in the current directory.")
        return
    
    # Open the first image to get dimensions
    sample_img = Image.open(os.path.join(current_dir, city_tiles[0]))
    tile_width, tile_height = sample_img.size
    
    # Create a blank canvas for the 5x5 grid
    grid_width = tile_width * width
    grid_height = tile_height * height
    composite = Image.new('RGBA', (grid_width, grid_height))
    
    # Place random tiles in the 5x5 grid
    for row in range(height):
        for col in range(width):
            # Pick a random tile
            random_tile = random.choice(city_tiles)
            img = Image.open(os.path.join(current_dir, random_tile))
            
            # Calculate position
            x = col * tile_width
            y = row * tile_height
            
            # Paste the tile
            composite.paste(img, (x, y))
    
    # Save the final composite image
    output_path = os.path.join(current_dir, "MSSBL_City.png")
    composite.save(output_path)
    print(f"Created composite image: {output_path}")

if __name__ == "__main__":
    create_city_composite()