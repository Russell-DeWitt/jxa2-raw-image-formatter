This C# program reads a saved RAW image (JXA2 12_bit RAW format image file) saved from an Allied Vision CSI2 Alvium 1500 C-120 camera running on a Jetson Orin NX board. 
This program uses the same GPU kernel functions that I use on the Jetson board.  Such that I use this program to develope and verify my GPU code that will run on the Orin NX. 
The source code is self-documented.  So it should be easy to follow.

![image](https://github.com/user-attachments/assets/27eee28c-a7db-4d9a-af79-acfbaece0d60)

Allied Vision's manufacturer description of camera: https://www.alliedvision.com/en/products/camera-series/alvium-1500-c-selector/?series=107 
Allied Vision's github for Jetson camera driver:  https://github.com/alliedvision/linux_nvidia_jetson 
