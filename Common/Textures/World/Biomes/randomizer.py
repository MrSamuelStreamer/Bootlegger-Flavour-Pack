import os
import re
import random
import glob
import cairosvg
import tempfile

import xml.etree.ElementTree as ET

# Register the namespace
ET.register_namespace('', "http://www.w3.org/2000/svg")
ET.register_namespace('inkscape', "http://www.inkscape.org/namespaces/inkscape")

# Define a list of colors to choose from
colors = [
    "#6b3409", "#5d4037", "#4e342e", "#3e2723", "#5d4037",  # Very dark browns
    "#424242", "#303030", "#212121", "#1a1a1a", "#262626",  # Dark grays
    "#3e3e3e", "#323232", "#2d2d2d", "#383838", "#404040",  # More grays
    "#465569", "#37474f", "#263238", "#1c313a", "#2c3e50",  # Dark blue-grays
    "#4d4d4d", "#555555", "#494949", "#3d3d3d", "#333333"   # Additional grays
    "#6b3409", "#5d4037", "#4e342e", "#3e2723", "#5d4037",  # Very dark browns
    "#424242", "#303030", "#212121", "#1a1a1a", "#262626",  # Dark grays
    "#3e3e3e", "#323232", "#2d2d2d", "#383838", "#404040",  # More grays
    "#465569", "#37474f", "#263238", "#1c313a", "#2c3e50",  # Dark blue-grays
    "#4d4d4d", "#555555", "#494949", "#3d3d3d", "#333333"   # Additional grays
    "#6b3409", "#5d4037", "#4e342e", "#3e2723", "#5d4037",  # Very dark browns
    "#424242", "#303030", "#212121", "#1a1a1a", "#262626",  # Dark grays
    "#3e3e3e", "#323232", "#2d2d2d", "#383838", "#404040",  # More grays
    "#465569", "#37474f", "#263238", "#1c313a", "#2c3e50",  # Dark blue-grays
    "#4d4d4d", "#555555", "#494949", "#3d3d3d", "#333333"   # Additional grays
    # Double up the above, to lazy to add proper weighting
    "#2a7a3d", "#2f9956"  # Darker grass greens
]

def randomize_svg(input_file):
    # Parse the SVG file
    tree = ET.parse(input_file)
    root = tree.getroot()
    
    # Find all rect elements with the inkscape:label="Square" attribute
    for elem in root.iter('{http://www.w3.org/2000/svg}rect'):
        if elem.get('{http://www.inkscape.org/namespaces/inkscape}label') == "Square":
            style = elem.get('style')
            if style:
                # Replace the fill color in the style attribute
                if 'fill:#' in style:
                    random_color = random.choice(colors)
                    new_style = re.sub(r'fill:#[0-9a-fA-F]{6}', f'fill:{random_color}', style)
                    elem.set('style', new_style)
    
    return tree

def get_next_filename():
    # Find existing files that match the pattern
    existing_files = glob.glob('MSSBL_City*.png')
    
    # Extract numbers from filenames
    numbers = []
    for file in existing_files:
        match = re.search(r'MSSBL_City(\d+)\.png', file)
        if match:
            numbers.append(int(match.group(1)))
    
    # Determine the next available number
    next_number = 1
    if numbers:
        next_number = max(numbers) + 1
    
    return f'MSSBL_City{next_number}.png'

def main():
    input_file = 'MSSBL_City_Template.svg'
    output_file = get_next_filename()
    
    try:
        # Convert the SVG to PNG
        
        # First get the randomized SVG
        modified_tree = randomize_svg(input_file)
        
        # Create a temporary file for the modified SVG
        with tempfile.NamedTemporaryFile(suffix='.svg', delete=False) as tmp:
            modified_tree.write(tmp.name, encoding='utf-8', xml_declaration=True)
            tmp_name = tmp.name
                
        # Convert SVG to PNG with 512x512 dimensions
        cairosvg.svg2png(url=tmp_name, write_to=output_file, output_width=512, output_height=512)
        
        # Delete temporary SVG file
        os.remove(tmp_name)
    except Exception as e:
        print(f"Error processing file: {e}")

if __name__ == "__main__":
    main()