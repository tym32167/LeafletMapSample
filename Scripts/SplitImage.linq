<Query Kind="Program">
  <Connection>
    <ID>dea421a9-bd95-49ce-927a-0b15f2263890</ID>
    <Persist>true</Persist>
    <Provider>System.Data.SqlServerCe.4.0</Provider>
    <AttachFileName>D:\Arma3\Arma3BEClient\Database.sdf</AttachFileName>
  </Connection>
  <Namespace>System.Drawing</Namespace>
  <Namespace>System.Drawing.Imaging</Namespace>
</Query>

void Main()
{
	var folder = @"E:\tests\WEB\LeafletMapSample\tiles";
	var sourceFile = @"E:\tests\WEB\LeafletMapSample\Img\Source.png";
	if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);
	var dicts = new Dictionary<string, int>();
	
	for (var i=0; i<5;i++)
	{
		dicts.Add((18-i).ToString(), (int)(Math.Pow(2, i)));
	}
	
	foreach(var p in dicts)
	{
		Process(p.Key, p.Value, folder, sourceFile);
	}	
}

void Process(string scale, int mult, string folder, string source)
{
	var destFolder = Path.Combine(folder, scale);
	if (!Directory.Exists(destFolder)) Directory.CreateDirectory(destFolder);
	var outpFile = Path.Combine(destFolder, "source.png");
	if (!File.Exists(outpFile))
		Resize(source, outpFile, 1.0/mult);
		
		
	var pattern = Path.Combine(destFolder, "tile_{0}_{1}.png");
	split(outpFile, pattern);
}



static void Resize(string inpFile, string outpFile, double mult)
{
	var dir  = Path.GetDirectoryName(outpFile);
	if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
	
	if (File.Exists(outpFile)) return;
	using(var bm = new Bitmap(inpFile))
	{
	    var w = bm.Width;
        var h = bm.Height;        
        
            var newW = (int) (mult*w);
            var newH = (int)(mult * h);
			newW.Dump();
			newH.Dump();
	
			using(var newBitmap = new Bitmap(newW, newH))
			{
               	using (var gr = Graphics.FromImage(newBitmap))
               	{
            		gr.DrawImage(bm, new Rectangle(0, 0, newW, newH), 
                   	 	new Rectangle(0, 0, w, h),                        
                   	 	GraphicsUnit.Pixel);			
                }
                newBitmap.Save(outpFile, ImageFormat.Png);
			}        
	}
}

void split(string source, string destPattern)
{
	var fname = source;//@"E:\tests\WEB\LeafletMapSample\Img\1.png";	
	var fpattern = destPattern;//@"E:\tests\WEB\LeafletMapSample\Img\tiles\tile_{0}_{1}.png";
	
	var div = 256;
	
	using(var bmap = Bitmap.FromFile(fname))
	{
		var xnum = bmap.Width / div + (bmap.Width%div>0?1:0);
		var ynum = bmap.Height / div + (bmap.Height%div>0?1:0);
		
		for (var i=0; i<xnum; i++)
		{
			for (var j=0; j<ynum; j++)
			{
				var filename = string.Format(fpattern, i, j);
				
				var wid = div;
				var hig = div;
				
				using (var bm = new Bitmap(wid, hig))
				{
					using (var gr = Graphics.FromImage(bm))
        			{
						gr.FillRectangle(Brushes.LightGray, 0, 0, wid, hig);							
						gr.DrawImage(bmap, new Rectangle(0, 0, wid, hig), new Rectangle(i*div, j*div, wid, hig), GraphicsUnit.Pixel);
					}
					
					bm.Save(filename);					
				}				
			}
		}
		
	}
}

// Define other methods and classes here
