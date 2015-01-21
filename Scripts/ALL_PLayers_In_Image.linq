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
	var mult = 1;
	int w = (int)  (250 * mult); 
	int h = (int) (25 * mult);
	
	int xtimes = 57;
	int ytimes = 558;
	
	var filename = @"E:\tests\WEB\LeafletMapSample\Img\Source.png";
	
	var nameRegex = new Regex("[A-Za-zА-Яа-я0-9]+", RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);
	
	var names = Players.Select(x=>new {x.Name, lastnames = x.PlayerHistories.Select(y=>y.Name).ToArray()}).ToList().SelectMany(x=>GetNames(x.Name, x.lastnames)).Where(x=> !string.IsNullOrEmpty(x) && !x.Contains("(OK)")).Where(x=> nameRegex.IsMatch(x) ).Distinct().OrderBy(x=>x).ToList();	
	
	names.Count.Dump();
	
	var baned = Bans.Where(x=>x.IsActive).Select(x=>new {x.Player.Name, lastnames = x.Player.PlayerHistories.Select(y=>y.Name).ToArray()}).ToList().SelectMany(x=>GetNames(x.Name, x.lastnames)).Where(x=>!string.IsNullOrEmpty(x)).Where(x=> nameRegex.IsMatch(x) ).Distinct().ToList();	
	var banedSet = new HashSet<string>(baned);
	
	var commented = Players.Select(x=>new {x.Name, lastnames = x.PlayerHistories.Select(y=>y.Name).ToArray(), x.Comment}).ToList()
	.Where(x=>!string.IsNullOrEmpty(x.Comment)).SelectMany(x=>GetNames(x.Name, x.lastnames)).Where(x=> nameRegex.IsMatch(x) ).Distinct().ToList();	
	var commentSet = new HashSet<string>(commented);
	
	int mapw = (int) (w*xtimes);
	int maph = (int) (h*ytimes);	
	
	using(var bm = new Bitmap(mapw, maph))
	{	
		using (var gr = Graphics.FromImage(bm))
        {
			gr.FillRectangle(Brushes.LightGray, 0, 0, w*xtimes, h*ytimes);
			
			var namescount = names.Count;
		
			for(var i=0; i<xtimes; i++)
			{
				for (var j=0; j<ytimes; j++)
				{
					var index = i * ytimes + j;
					
					if (index < namescount)
					{
						var x = i*w;
						var y = j*h;
						
						var name = names[index];
						if (banedSet.Contains(name))
						{
							gr.FillRectangle(Brushes.DarkGreen, x, y, w, h);
							gr.DrawString(name , new Font("Comic sans", (int) (14* mult)), Brushes.LightGray, x, y);		
						}
						else if (commentSet.Contains(name))
						{
							gr.FillRectangle(Brushes.DarkGray, x, y, w, h);
							gr.DrawString(name , new Font("Comic sans", (int) (14* mult)), Brushes.Black, x, y);		
						}
						else
						{
							gr.FillRectangle(Brushes.LightGray, x, y, w, h);
							gr.DrawString(name , new Font("Comic sans", (int) (14* mult)), Brushes.Black, x, y);						
						}
											
					}
				}
			}						
		}
		bm.Save(filename, ImageFormat.Png);
	}
}


private string[] GetNames(string name, string[] lastnames)
{
	var names = new List<string>(lastnames);
	names.Add(name);
	return names.ToArray();
}

// Define other methods and classes here
