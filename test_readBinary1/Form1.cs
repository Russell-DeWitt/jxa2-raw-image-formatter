using System.Drawing.Imaging;
using System.Formats.Asn1;
using System.IO;
using System.Linq.Expressions;
using System.Text;
using System.Windows.Forms;
using System.Xml.Linq;
using static System.Windows.Forms.LinkLabel;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Menu;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace test_readBinary1
{
    public partial class Form1 : Form
    {
        String initialPath = "C:\\"; 
        String inPathname = "";
        String outPathname = "";
        int bmp_width = 0;
        int bmp_height = 0;
        String path = "";
        String filename = "";
        String fname = "";
        String ftype = "";
        long fileLength = 0;
        long arrayLength = 0;
        const int max_bmp_width = 1280;
        const int max_bmp_height = 960;
        const int max_arrayLength = max_bmp_width * max_bmp_height;
        uint[] frame = new uint[max_arrayLength];
        long fileLen = 0;
        Bitmap myBitmap = new Bitmap(1, 1);
        int defaultFormWidth = 0;
        int defaultFormHeight = 0;
        public struct myColor
        {
            public myColor(uint r, uint g, uint b)
            {
                R = r;
                G = g;
                B = b;
            }
            public uint R { get; }
            public uint G { get; }
            public uint B { get; }
            public override string ToString() => $"({R}, {G}, {B})";
        }

        public Form1()
        {
            InitializeComponent();
            defaultFormWidth = this.Width;
            defaultFormHeight = this.Height;
        }

        private void button1_Click(object sender, EventArgs e) 
        {
            var filePath = string.Empty;

            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = initialPath;
                openFileDialog.Filter = "RAW (*.raw)|*.raw";
                openFileDialog.Title = "Open RAW File (must be Allied Vision RAW JXA2 format)";
                openFileDialog.RestoreDirectory = true;
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    //Get the path of specified file
                    inPathname = openFileDialog.FileName;
                    if (checkPathname() == true)
                    {
                        processRawFile();
                    }
                }
            }
        }

        private void processRawFile()
        {
            parsePathname();
            ReadFile(inPathname);

            //Save original RAW file values in in Hex+Binary format (for debugging)
            if (checkBox1.Checked == true)
            {
                SaveAsHexFile(frame, path + "\\" + fname + "(original-raw-file).hex");
            }

            //////////////////////////////////////////////////////////////////////////////////
            //  step-1: shift each 12bit value right 3 bits and mask the lower 12 bits.
            //////////////////////////////////////////////////////////////////////////////////
            shiftAndMask();

            //Save the 'step-1' bit-corrected 12-bit RAW values as a binary file
            if (checkBox2.Checked == true)
            {
                SaveAsBinaryFile(frame, path + "\\" + fname + "(bit-corrected).bin");
            }

            //Save the 'step-1' bit-corrected 12-bit RAW values in Hex+Binary format (for debugging)
            if (checkBox3.Checked == true)
            {
                SaveAsHexFile(frame, path + "\\" + fname + "(bit-corrected).hex");
            }

            //////////////////////////////////////////////////////////////////////////////////
            //  step-2: Do 'demozaic' operation to generate 12-bit R, G, B values
            //////////////////////////////////////////////////////////////////////////////////
            doDemozaic();

            //Save as a viewable 32bit RGBA TIFF file
            if (checkBox6.Checked == true)
            {
                outPathname = path + "\\" + fname + "(32bit_rgba_tiff).tif";
                myBitmap.Save(outPathname, ImageFormat.Tiff);
                textBox_debug.AppendText("Saved file " + outPathname + Environment.NewLine);
            }

        }

        private void SaveAsBinaryFile(uint[] arry, String outPathname)
        {
            using (BinaryWriter writer = new BinaryWriter(File.Open(outPathname, FileMode.Create)))
            {
                for (int i = 0; i < arrayLength; ++i)
                {
                    writer.Write((UInt16)arry[i]);   
                }
            }
            textBox_debug.AppendText("Saved file " + outPathname + Environment.NewLine);
        }

        private void SaveAsHexFile(uint[] arry, String outPathname)
        {
            using (StreamWriter outputFile = new StreamWriter(outPathname))
            {
                for (int i = 0; i < arrayLength; ++i)
                {
                    uint val = arry[i];
                    String hex = $"0x{val:X4}";
                    String bin = Convert.ToString(val, 2).PadLeft(16, '0');
                    outputFile.WriteLine(hex + "\t" + bin);
                }
            }
            textBox_debug.AppendText("Saved file " + outPathname + Environment.NewLine);
        }


        private void ReadFile(String fileName)
        {
            if (File.Exists(fileName))
            {
                //GET FILE LENGTH IN BYTES
                FileInfo FileVol = new FileInfo(fileName);
                fileLen = FileVol.Length;
                textBox_debug.AppendText("File length is " + fileLen + Environment.NewLine);
                //FileInfo fi = new FileInfo(fileName);

                textBox_debug.AppendText("Reading input RAW file: " + fileName + Environment.NewLine);
                using (var stream = File.Open(fileName, FileMode.Open))
                {
                    using (var reader = new BinaryReader(stream))
                    {
                        for (int i = 0; i < arrayLength; i++)
                        {
                            frame[i] = (uint)reader.ReadUInt16();
                        }
                    }

                }
            }
        }

        private void shiftAndMask()
        {
            // Allied-Vision's custom 12-bit bit order is...
            // X B11 B10 B9  B8 B7 B6 B5  B4 B3 B2 B1  B0 X X X
            // shift right 3 bits, it becomes
            // X X X X  B11 B10 B9 B8  B7 B6 B5 B4  B3 B2 B1 B0
            // then AND with MASK (binary)0000 1111 1111 1111 = 0x0FFF 
            //
            for (int i = 0; i < arrayLength; ++i)
            {
                uint word = frame[i];
                uint shifted = word >> 3;
                uint mask = 0b_0000_1111_1111_1111;
                word = shifted & mask;
                frame[i] = word;
            }
        }


        private void doDemozaic()
        {

            using (BinaryWriter binWriter = new BinaryWriter(File.Open(path + "\\" + fname + "(12-bit_rgb).bin", FileMode.Create)))
            {
                int indx = 0;
                using (StreamWriter hexWriter = new StreamWriter(path + "\\" + fname + "(12-bit_rgb).hex"))
                {
                    for (int y = 0; y < bmp_height; y++)     // walk thru rows (the 'y' values)
                    {
                        for (int x = 0; x < bmp_width; x++)  // walk thru cols (the 'x' values)
                        {
                            myColor myCol = getColor(x, y);
                            ++indx;
                            Color color = Color.FromArgb(255, (Int32)myCol.R >> 4, (Int32)myCol.G >> 4, (Int32)myCol.B >> 4);
                            myBitmap.SetPixel(x, y, color);

                            if (checkBox4.Checked == true) //Save 'step-2' demozaiced 12-bit RGB values as a binary file
                            {
                                binWriter.Write((UInt16)myCol.R);   //use Write overload 'Write(UInt16)':
                                binWriter.Write((UInt16)myCol.G);
                                binWriter.Write((UInt16)myCol.B);
                            }
                            if (checkBox5.Checked == true) //Save 'step-2' demozaiced 12-bit RGB values in Hex+Binary format (for debugging)
                            {
                                uint val = (UInt16)myCol.R;
                                String line = $"[0x{val:X4}" + "\t";
                                line += Convert.ToString(val, 2).PadLeft(16, '0') + "\t";
                                val = (UInt16)myCol.G;
                                line += $", 0x{val:X4}" + "\t";
                                line += Convert.ToString(val, 2).PadLeft(16, '0') + "\t";
                                val = (UInt16)myCol.B;
                                line += $", 0x{val:X4}]" + "\t";
                                line += Convert.ToString(val, 2).PadLeft(16, '0') + "\t";
                                hexWriter.WriteLine(line);
                            }
                        }
                    }
                }
            }
        }


        ////////////////////////////////////////////////////
        // FUNCTION:  myColor getColor(int x, int y)
        // DESCRIPTION: Input the x,y coordinate of the captured (12-bit data) RAW GRGR,BGBG camera frame,
        // Return the 12-bit red, green, and blue values that correspond to that x,y coordinate.
        //
        // Description of bayer conversion:
        // You have a 1280x960 array of 12-bit uint's.  frame_width = 1280; frame_height = 960;
        // At position x,y in the array, it may be a R or G or B value.
        // To get the red value at x and y,
        // you have to interpret the distance to nearest red values in X and Y directions
        // and weight them according to distance.
        //   col:  0 1 2 3 4 5 6 7
        // row 0:  G R G R G R G R 
        // row 1:  B G B G B G B G 
        // row 2:  G R G R G R G R 
        // row 3:  B G B G B G B G 
        //  
        // so first I need a function for each R, G, B 
        //
        // case1, if I am on an odd BGBG row, and an odd column
        //   eg location x=1,y=1, and I want the R, G, B values...
        //   col:  0 1 2 3 4 5 6 7
        // row 0:  G R G R G R G R 
        // row 1:  B[G]B G B G B G 
        // row 2:  G R G R G R G R 
        // row 3:  B G B G B G B G 
        // Red is just the avg  of north and south values
        // Green is easy, it just that value
        // Blue is the avg of left and right values
        //
        // case2, if I am location x=1,y=2
        // row 0:  G R G R G R G R 
        // row 1:  B G[B]G B G B G 
        // row 2:  G R G R G R G R 
        // row 3:  B G B G B G B G 
        // Red is the avg of surrounding 4 corner values
        // Green is the avg of left and right values
        // Blue is easy, it just that value 
        //
        // on col 3 and so on, case 1 or 2 just repeats 
        //
        // case3, if I am on an even GRGR row, and an odd column
        //   eg location x=2,y=1, and I want the R, G, B values...
        //   col:  0 1 2 3 4 5 6 7
        // row 0:  G R G R G R G R 
        // row 1:  B G B G B G B G 
        // row 2:  G R[G]R G R G R 
        // row 3:  B G B G B G B G 
        // Green is easy, it just that value
        // Blue is the avg of of north and south values 
        // Red is the avg left and right values 
        //
        // case4, if I am on an even GRGR row, and an even column
        //   eg location x=2,y=2, and I want the R, G, B values...
        //   col:  0 1 2 3 4 5 6 7
        // row 0:  G R G R G R G R 
        // row 1:  B G B G B G B G 
        // row 2:  G R G[R]G R G R 
        // row 3:  B G B G B G B G 
        // Red is easy, it just that value
        // Green is the avg left and right values 
        // Blue is the avg  of surrounding 4 corner values 
        //
        // on col 3 and so on, case 3 or 4 just repeats 
        //
        // on edges, you will detect an out of bound row/col index
        //  maybe it's easier to just detect special cases for each.
        //
        // first you need to know which case you are int based on x,y index
        // if row odd and col odd you have case 1
        private myColor getColor(int x, int y)
        {
            uint r = 0;
            uint g = 0;
            uint b = 0;

            //////////////////////////////
            // THE 4 CORNER CASES
            //////////////////////////////
            int indx_topLeft = 0;
            int indx_topRight = bmp_width - 1;
            int indx_botLeft = bmp_width * (bmp_height - 1);
            int indx_botRight = (bmp_width * bmp_height) - 1; //(8*4)-1=31
            if ((x == 0) && (y == 0))   //top left corner case
            {
                //   col:  0 1 2 3 4 5 6 7
                // row 0: [G]R G R G R G R 
                // row 1:  B G B G B G B G 
                // row 2:  G R G R G R G R 
                // row 3:  B G B G B G B G 
                int indx_below = bmp_width;
                r = frame[1];            // red val is to the right at (1,0)
                g = frame[indx_topLeft]; // Green val is at (0,0)
                b = frame[indx_below];   //blue is below at (0,1)
            }
            else if ((x == bmp_width - 1) && (y == 0)) //top right corner case
            {
                //   col:  0 1 2 3 4 5 6 7
                // row 0:  G R G R G R G[R] 
                // row 1:  B G B G B G B G 
                // row 2:  G R G R G R G R 
                // row 3:  B G B G B G B G 

                int indx_left = indx_topRight - 1;
                int indx_below = 2 * indx_topRight;
                r = frame[indx_topRight]; // red val is just the current pixel at (0,current_indx)
                g = (frame[indx_left] + frame[indx_below]) / (uint)2; // Green val is the avg of one to left and below
                b = frame[indx_below - 1];     //blue is below-left at (indx_left,indx_below)
            }
            else if ((x == 0) && (y == bmp_height - 1)) //bottom left corner case
            {
                //   col:  0 1 2 3 4 5 6 7
                // row 0:  G R G R G R G R  
                // row 1:  B G B G B G B G 
                // row 2:  G R G R G R G R 
                // row 3: [B]G B G B G B G 
                // bmp_width = 8;  bmp_height = 4;  
                int indx_above = indx_botLeft - bmp_height; //8*2=16
                int indx_right = indx_botLeft + 1;          //24+1=25
                int indx_aboveRight = indx_above + 1;
                r = frame[indx_aboveRight]; // red val is just the current pixel at (0,current_indx)
                g = (frame[indx_above] + frame[indx_right]) / (uint)2; // Green val is the avg of one to left and below
                b = frame[indx_botLeft];     //blue is index simply below-left
            }
            else if ((x == bmp_width - 1) && (y == bmp_height - 1)) //bottom right corner case
            {
                //   col:  0 1 2 3 4 5 6 7
                // row 0:  G R G R G R G R  
                // row 1:  B G B G B G B G 
                // row 2:  G R G R G R G R 
                // row 3:  B G B G B G B[G] 
                // bmp_width = 8;  bmp_height = 4;  
                int indx_above = indx_botRight - bmp_width; //8*2=16
                int indx_left = indx_botRight - 1;          //24+1=25
                int indx_aboveLeft = indx_above - 1;
                r = frame[indx_above];     // red val is just the current pixel at (0,current_indx)
                g = frame[indx_botRight];  // Green val is the avg of one to left and below
                b = frame[indx_left];      //blue is index simply below-left
            }
            //////////////////////////////
            // THE 4 EDGE CASES           
            //////////////////////////////
            // LEFT EDGE case
            else if (x == 0)
            {
                if (isOdd(y))  //odd rows
                {
                    //   col:  0 1 2 3 4 5 6 7
                    // row 0:  G R G R G R G R 
                    // row 1: [B]G B G B G B G 
                    // row 2:  G R G R G R G R 
                    // row 3:  B G B G B G B G
                    int indx_xy = x + (y * bmp_width);
                    int indx_above = indx_xy - bmp_width;
                    int indx_below = indx_xy + bmp_width;
                    int indx_right = indx_xy + 1;
                    int indx_aboveRight = indx_above + 1;
                    int indx_belowRight = indx_below + 1;
                    r = (frame[indx_aboveRight] + frame[indx_belowRight]) / 2;
                    g = (frame[indx_above] + frame[indx_right] + frame[indx_below]) / 3;
                    b = frame[indx_xy];      //blue is val at (x,y)
                }
                else    //even rows
                {
                    //   col:  0 1 2 3 4 5 6 7
                    // row 0:  G R G R G R G R 
                    // row 1:  B G B G B G B G 
                    // row 2: [G]R G R G R G R 
                    // row 3:  B G B G B G B G
                    int indx_xy = x + (y * bmp_width);
                    int indx_right = indx_xy + 1;
                    int indx_above = indx_xy - bmp_width;
                    int indx_below = indx_xy + bmp_width;
                    r = frame[indx_right];   // red is the value to the right at (x+1,y)
                    g = frame[indx_xy];      // Green is val at (x,y)
                    b = (frame[indx_above] + frame[indx_below]) / 2;    // blue is below at (0,1)
                }
            }
            // RIGHT EDGE case
            else if (x == bmp_width - 1)
            {
                if (isOdd(y))  //odd rows
                {
                    //   col:  0 1 2 3 4 5 6 7
                    // row 0:  G R G R G R G R 
                    // row 1:  B G B G B G B[G] 
                    // row 2:  G R G R G R G R 
                    // row 3:  B G B G B G B G
                    int indx_xy = x + (y * bmp_width);
                    int indx_above = indx_xy - bmp_width;
                    int indx_below = indx_xy + bmp_width;
                    int indx_left = indx_xy - 1;
                    r = (frame[indx_above] + frame[indx_below]) / 2;
                    g = frame[indx_xy];
                    b = frame[indx_left];
                }
                else    //even rows
                {
                    //   col:  0 1 2 3 4 5 6 7
                    // row 0:  G R G R G R G R 
                    // row 1:  B G B G B G B G 
                    // row 2:  G R G R G R G[R] 
                    // row 3:  B G B G B G B G
                    int indx_xy = x + (y * bmp_width);
                    int indx_left = indx_xy - 1;
                    int indx_above = indx_xy - bmp_width;
                    int indx_below = indx_xy + bmp_width;
                    int indx_aboveLeft = indx_above - 1;
                    int indx_belowLeft = indx_below - 1;
                    r = frame[indx_xy];
                    g = (frame[indx_above] + frame[indx_left] + frame[indx_below]) / 3;     // Green is avg of 3: above, left, below
                    b = (frame[indx_aboveLeft] + frame[indx_belowLeft]) / 2;    // blue is below at (0,1)
                }
            }
            // TOP EDGE case
            else if (y == 0)
            {
                if (isOdd(y))  //odd columns
                {
                    //   col:  0 1 2 3 4 5 6 7
                    // row 0:  G[R]G R G R G R 
                    // row 1:  B G B G B G B G 
                    // row 2:  G R G R G R G R 
                    // row 3:  B G B G B G B G
                    int indx_xy = x + (y * bmp_width);
                    int indx_below = indx_xy + bmp_width;
                    int indx_left = indx_xy - 1;
                    int indx_right = indx_xy + 1;
                    int indx_belowLeft = indx_below - 1;
                    int indx_belowRight = indx_below + 1;
                    r = frame[indx_xy];
                    g = (frame[indx_left] + frame[indx_right] + frame[indx_below]) / 3;
                    b = (frame[indx_belowLeft] + frame[indx_belowRight]) / 2;
                }
                else    //even columns
                {
                    //   col:  0 1 2 3 4 5 6 7
                    // row 0:  G R[G]R G R G R 
                    // row 1:  B G B G B G B G 
                    // row 2:  G R G R G R G R 
                    // row 3:  B G B G B G B G
                    int indx_xy = x + (y * bmp_width);
                    int indx_below = indx_xy + bmp_width;
                    int indx_left = indx_xy - 1;
                    int indx_right = indx_xy + 1;
                    int indx_belowLeft = indx_below - 1;
                    int indx_belowRight = indx_below + 1;
                    r = (frame[indx_left] + frame[indx_right]) / 2;
                    g = frame[indx_xy];
                    b = (frame[indx_belowLeft] + frame[indx_belowRight]) / 2;
                }
            }
            // BOTTOM EDGE case
            else if (y == bmp_height - 1)
            {
                if (isOdd(y))  //odd columns
                {
                    //   col:  0 1 2 3 4 5 6 7
                    // row 0:  G R G R G R G R 
                    // row 1:  B G B G B G B G 
                    // row 2:  G R G R G R G R 
                    // row 3:  B[G]B G B G B G
                    int indx_xy = x + (y * bmp_width);
                    int indx_above = indx_xy - bmp_width;
                    int indx_left = indx_xy - 1;
                    int indx_right = indx_xy + 1;
                    r = frame[indx_above];
                    g = frame[indx_xy];
                    b = (frame[indx_left] + frame[indx_right]) / 2;
                }
                else    //even columns
                {
                    //   col:  0 1 2 3 4 5 6 7
                    // row 0:  G R G R G R G R 
                    // row 1:  B G B G B G B G 
                    // row 2:  G R G R G R G R 
                    // row 3:  B G[B]G B G B G
                    int indx_xy = x + (y * bmp_width);
                    int indx_above = indx_xy - bmp_width;
                    int indx_left = indx_xy - 1;
                    int indx_right = indx_xy + 1;
                    int indx_aboveLeft = indx_above - 1;
                    int indx_aboveRight = indx_above + 1;
                    r = (frame[indx_aboveLeft] + frame[indx_aboveRight]) / 2;
                    g = (frame[indx_left] + frame[indx_right] + frame[indx_above]) / 3;
                    b = frame[indx_xy];
                }
            }
            //////////////////////////////
            // CASE1: X-COL=ODD and Y-ROW=ODD 
            else if (isOdd(x) && isOdd(y))
            {
                // case1, on an odd row (a BGBG row), and an odd column (the G pixel)
                //   eg location x=1,y=1, and I want the R, G, B values...
                //   col:  0 1 2 3 4 5 6 7
                // row 0:  G R G R G R G R 
                // row 1:  B[G]B G B G B G 
                // row 2:  G R G R G R G R 
                // row 3:  B G B G B G B G 
                int indx_xy = x + (y * bmp_width);
                int indx_above = indx_xy - bmp_width;
                int indx_below = indx_xy + bmp_width;
                int indx_left = indx_xy - 1;
                int indx_right = indx_xy + 1;
                r = (frame[indx_above] + frame[indx_below]) / 2;// Red is the avg  of above and below values
                g = frame[indx_xy];                             // Green is the value at (x,y)
                b = (frame[indx_left] + frame[indx_right]) / 2; // Blue is the avg of left and right values
            }
            //////////////////////////////
            // CASE2: X-COL=EVEN and Y-ROW=ODD 
            else if (isEven(x) && isOdd(y))
            {
                // case2, on an odd row (a BGBG row), and an even column (the B pixel)
                //   eg location x=1,y=2, and I want the R, G, B values...
                //   col:  0 1 2 3 4 5 6 7
                // row 0:  G R G R G R G R 
                // row 1:  B G[B]G B G B G 
                // row 2:  G R G R G R G R 
                // row 3:  B G B G B G B G 
                int indx_xy = x + (y * bmp_width);
                int indx_above = indx_xy - bmp_width;
                int indx_below = indx_xy + bmp_width;
                int indx_left = indx_xy - 1;
                int indx_right = indx_xy + 1;
                int indx_aboveLeft = indx_above - 1;
                int indx_belowLeft = indx_below - 1;
                int indx_aboveRight = indx_above + 1;
                int indx_belowRight = indx_below + 1;
                r = (frame[indx_aboveLeft] + frame[indx_belowLeft] + frame[indx_aboveRight] + frame[indx_belowRight]) / 4;
                g = (frame[indx_above] + frame[indx_below] + frame[indx_left] + frame[indx_right]) / 4;
                b = frame[indx_xy];  // Green is the value at (x,y)
            }
            //////////////////////////////
            // CASE3: X-COL=ODD and Y-ROW=EVEN
            else if (isOdd(x) && isEven(y))
            {
                // case3, on an even row (a GRGR row), and an odd column (the R pixel)
                //   eg location x=1,y=2, and I want the R, G, B values...
                //   col:  0 1 2 3 4 5 6 7
                // row 0:  G R G R G R G R 
                // row 1:  B G B G B G B G 
                // row 2:  G[R]G R G R G R 
                // row 3:  B G B G B G B G 
                int indx_xy = x + (y * bmp_width);
                int indx_above = indx_xy - bmp_width;
                int indx_below = indx_xy + bmp_width;
                int indx_left = indx_xy - 1;
                int indx_right = indx_xy + 1;
                int indx_aboveLeft = indx_above - 1;
                int indx_belowLeft = indx_below - 1;
                int indx_aboveRight = indx_above + 1;
                int indx_belowRight = indx_below + 1;
                r = frame[indx_xy];  // Red is the value at (x,y)
                g = (frame[indx_above] + frame[indx_below] + frame[indx_left] + frame[indx_right]) / 4;
                b = (frame[indx_aboveLeft] + frame[indx_belowLeft] + frame[indx_aboveRight] + frame[indx_belowRight]) / 4;
            }
            //////////////////////////////
            // CASE4: X-COL=EVEN and Y-ROW=EVEN 
            else if (isEven(x) && isEven(y))
            {
                // case4, on an even row (a GRGR row), and an even column (the G pixel)
                //   eg location x=2,y=2, and I want the R, G, B values...
                //   col:  0 1 2 3 4 5 6 7
                // row 0:  G R G R G R G R 
                // row 1:  B G B G B G B G 
                // row 2:  G R[G]R G R G R 
                // row 3:  B G B G B G B G 
                int indx_xy = x + (y * bmp_width);
                int indx_above = indx_xy - bmp_width;
                int indx_below = indx_xy + bmp_width;
                int indx_left = indx_xy - 1;
                int indx_right = indx_xy + 1;
                int indx_aboveLeft = indx_above - 1;
                int indx_belowLeft = indx_below - 1;
                int indx_aboveRight = indx_above + 1;
                int indx_belowRight = indx_below + 1;
                r = (frame[indx_left] + frame[indx_right]) / 2;
                g = frame[indx_xy];  // Green is the value at (x,y)
                b = (frame[indx_above] + frame[indx_below]) / 2;
            }
            var col = new myColor(r, g, b);
            return col;
        }

        private bool isEven(int val)
        {
            if (val % 2 == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private bool isOdd(int val)
        {
            if (val % 2 == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }


        private Boolean checkPathname()
        {
            textBox_debug.AppendText(Environment.NewLine + "Checking file '" + inPathname + "'" + Environment.NewLine);
            FileInfo FileVol = new FileInfo(inPathname);
            fileLength = FileVol.Length;
            arrayLength = fileLength / 2;
            textBox_debug.AppendText("File size is " + fileLength + Environment.NewLine);
            switch (fileLength)
            {
                case 2457600:
                    bmp_width = 1280;
                    bmp_height = 960;
                    textBox_debug.AppendText("Frame size is " + bmp_width + "x" + bmp_height + Environment.NewLine);
                    break;
                case 614400:
                    bmp_width = 640;
                    bmp_height = 480;
                    textBox_debug.AppendText("Frame size is " + bmp_width + "x" + bmp_height + Environment.NewLine);
                    break;
                case 245760:
                    bmp_width = 384;
                    bmp_height = 320;
                    textBox_debug.AppendText("Frame size is " + bmp_width + "x" + bmp_height + Environment.NewLine);
                    break;
                case 153600:
                    bmp_width = 320;
                    bmp_height = 240;
                    textBox_debug.AppendText("Frame size is " + bmp_width + "x" + bmp_height + Environment.NewLine);
                    break;
                case 98304:
                    bmp_width = 256;
                    bmp_height = 192;
                    textBox_debug.AppendText("Frame size is " + bmp_width + "x" + bmp_height + Environment.NewLine);
                    break;
                case 61440:
                    bmp_width = 192;
                    bmp_height = 160;
                    textBox_debug.AppendText("Frame size is " + bmp_width + "x" + bmp_height + Environment.NewLine);
                    break;
                case 34816:
                    bmp_width = 128;
                    bmp_height = 136;
                    textBox_debug.AppendText("Frame size is " + bmp_width + "x" + bmp_height + Environment.NewLine);
                    break;
                case 30720:
                    bmp_width = 128;
                    bmp_height = 120;
                    textBox_debug.AppendText("Frame size is " + bmp_width + "x" + bmp_height + Environment.NewLine);
                    break;
                default:
                    bmp_width = 0;
                    bmp_height = 0;
                    textBox_debug.ForeColor = Color.Red;
                    textBox1.Font = new Font(textBox1.Font, FontStyle.Bold);
                    textBox_debug.AppendText("#############################################################################" + Environment.NewLine);
                    textBox_debug.AppendText("#### Error! The selected file " + inPathname + Environment.NewLine);
                    textBox_debug.AppendText("#### does not appear to be type RAW JXA2 format." + Environment.NewLine);
                    textBox_debug.AppendText("#### Please choose another file." + Environment.NewLine);
                    textBox_debug.AppendText("##############################################################################" + Environment.NewLine);
                    textBox_debug.ForeColor = Color.Black;
                    textBox1.Font = new Font(textBox1.Font, FontStyle.Regular);
                    return false;
            }
            
            myBitmap = new Bitmap(bmp_width, bmp_height);
            pictureBox1.Width = myBitmap.Width;
            pictureBox1.Height = myBitmap.Height;
            pictureBox1.Image = myBitmap;
            int newWidth = pictureBox1.Width + pictureBox1.Location.X + 30;
            int formWidth = (newWidth > defaultFormWidth) ? newWidth : defaultFormWidth;
            int newHeight = pictureBox1.Height + pictureBox1.Location.Y + 50;
            int formHeight = (newHeight > defaultFormHeight) ? newHeight : defaultFormHeight;
            Size formSize = new Size(formWidth, formHeight);
            this.Size = formSize;
            return true;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////
        // Split the pathname "C:\\TEMP\\JXA2_1280x960.raw"
        // into 3 parts:  "C:\\TEMP\\" and "JXA2_1280x960" and "raw" 
        private void parsePathname()
        {
            int idx = inPathname.LastIndexOf('\\');
            if (idx >= 0)
            {
                path = inPathname.Substring(0, idx);
                filename = inPathname.Substring(idx + 1, (inPathname.Length) - idx - 1);
            }
            else
            {
                textBox_debug.AppendText("Warning! path is empty." + Environment.NewLine);
                path = "";
                filename = inPathname;
                textBox_debug.AppendText("pathname = '" + inPathname + "'" + Environment.NewLine);
                textBox_debug.AppendText("path     = '" + path + "'" + Environment.NewLine);
                textBox_debug.AppendText("filename = '" + filename + "'" + Environment.NewLine);
            }
            idx = filename.LastIndexOf('.');
            if (idx >= 0)
            {
                fname = filename.Substring(0, idx);
                ftype = filename.Substring(idx + 1, (filename.Length) - idx - 1);
            }
            else
            {
                textBox_debug.AppendText("Warning! file type is missing.  Filename is missing the '.'" + Environment.NewLine);
                fname = filename;
                ftype = ftype = "";
                textBox_debug.AppendText("fname    = '" + fname + "'" + Environment.NewLine);
                textBox_debug.AppendText("ftype    = '" + ftype + "'" + Environment.NewLine);
                textBox_debug.AppendText("pathname = '" + path + "\\" + fname + "." + ftype + "'" + Environment.NewLine);
            }
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}


