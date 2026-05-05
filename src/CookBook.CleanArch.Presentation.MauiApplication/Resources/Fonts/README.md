# Icon Font Update Guide

1. To add custom icons, go to https://icomoon.io/app/#/select and paste the current `selection.json` file content.
2. Select the icons you want to add, then click **"Generate Font"** from the bottom right.
3. Update the hexadecimal code (or corresponding character value) for each newly added icon, then click **"Download"**.
4. Download the generated ZIP file, extract it, and copy the updated `icomoon.ttf` and `selection.json` files into this folder.
5. Update `IconKeys.cs` with the newly added icons. The character (ASCII value) must match the values defined in step 3.
