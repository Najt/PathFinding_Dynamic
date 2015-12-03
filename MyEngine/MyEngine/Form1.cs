using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace MyEngine
{
    
    
    public partial class Form1 : Form
    {
        Bitmap image;
        Bitmap layer2;
        static string assetsPath = "Assets\\";
        Random r = new Random();
        Graphics graphics;
        Graphics l2graphics;
        static public int mapSize = 64;
        PathFinder pathfinder = new PathFinder();
        int[,] map = new int[mapSize, mapSize];
        static int tileSize = 36;
        Timer timer;
        bool inited = false;

        GameObject snake = new GameObject(tileSize);
        TileRescure groundTile = new TileRescure(Image.FromFile(assetsPath+"tile.png"),tileSize);
        TileRescure roadTile = new TileRescure(assetsPath+"Road",tileSize);
        bool showGrid = false;
        public Form1()
        {
            InitializeComponent();
            SetDesktopLocation(0, 0);
            SetDisplayRectLocation(0, 0);
            snake.PosX = tileSize * 5;
            snake.PosY = tileSize * 5;
            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    map[i, j] = r.Next(4);
                    if (i >= 5 && i <= 7) {
                        if (j>= 5 && j<=7) {
                            map[i, j] = 5;
                            int id = (i * map.GetLength(0)) + j;
                            pathfinder.run(id);
                        }
                    }
                }
            }
            initRender();
            dynamicBox.MouseClick += new MouseEventHandler(clickHandler);
            timer = new Timer();
            timer.Interval=10;
            timer.Tick += new EventHandler(tickEvent);
            timer.Start();
        }
        private void resied(Object sender, EventArgs e){
            initRender();
        }
        private void tickEvent(object sender, EventArgs e) {
            render();
        }
        public void initRender() {
            inited = false;
            Console.WriteLine("Init render");
            renderBox.Location = new Point(0, 0);
            renderBox.Width = Width;
            renderBox.Height = Height;

            dynamicBox.Location = new Point(0, 0);
            dynamicBox.Parent = renderBox;
            dynamicBox.BackColor = Color.Transparent;
            dynamicBox.Height = Height;
            dynamicBox.Width = Width;

            layer2 = new Bitmap(dynamicBox.Width, dynamicBox.Height);
            l2graphics = Graphics.FromImage(layer2);
            l2graphics.Clear(Color.Transparent);
            dynamicBox.Image = layer2;

            image = new Bitmap(renderBox.Width, renderBox.Height);
            graphics = Graphics.FromImage(image);
            graphics.Clear(Color.Black);
            renderBox.Image = image;

            label1.Left = map.GetLength(0) * tileSize;
            label1.Parent = renderBox;
            label1.BackColor = Color.Transparent;

            render();
        }
        private void onClose(object sender, FormClosingEventArgs e)
        {
            Console.WriteLine("event handled");
            graphics.Dispose();
            timer.Stop();
            timer.Dispose();
        }
        private int[] roadSelect(int x, int y) {
            int[] a = new int[2];
            bool left = x > 0 && map[x - 1, y] == 5;
            bool right = x < map.GetLength(0)-1 && map[x + 1, y] == 5;
            bool up = y > 0 && map[x, y-1] == 5;
            bool down = y < map.GetLength(1)-1 && map[x, y+1] == 5;

            if (!up && !down && !right && !left) {
                a[0] = 4;
                a[1] = 0;
            }
            if ((up || down) && !right && !left)
            {
                a[0] = 0;
                a[1] = 0;
            }
            if (!up && !down && (right || left))
            {
                a[0] = 0;
                a[1] = 1;
            }
            if (up && down && right && left)
            {
                a[0] = 3;
                a[1] = 0;
            }
            if (up && down && right && !left)
            {
                a[0] = 2;
                a[1] = 0;
            }
            if (!up && down && right && left)
            {
                a[0] = 2;
                a[1] = 1;
            }
            if (up && down && !right && left)
            {
                a[0] = 2;
                a[1] = 2;
            }
            if (up && !down && right && left)
            {
                a[0] = 2;
                a[1] = 3;
            }
            if (!up && down && !right && left)
            {
                a[0] = 1;
                a[1] = 1;
            }
            if (!up && down && right && !left)
            {
                a[0] = 1;
                a[1] = 0;
            }
            if (up && !down && !right && left)
            {
                a[0] = 1;
                a[1] = 2;
            }
            if (up && !down && right && !left)
            {
                a[0] = 1;
                a[1] = 3;
            }
            return a;
        }
        
        private void render() {
            l2graphics.Clear(Color.Transparent);
            snake.run(pathfinder);
            if (!inited)
            {
                for (int i = 0; i < map.GetLength(0); i++)
                {
                    for (int j = 0; j < map.GetLength(1); j++)
                    {
                        if (map[i, j] < 5)
                        {
                            graphics.DrawImage(groundTile.getTile(map[i, j]), i * tileSize, j * tileSize);
                        }
                        else if (map[i, j] == 5)
                        {
                            graphics.DrawImage(groundTile.getTile(0), i * tileSize, j * tileSize);
                        }
                    }
                }
                inited = true;
                renderBox.Image = image;
            }
            for (int i = 0; i < pathfinder.roads.Count; i++)
            {
                int x = pathfinder.roads[i].Id % mapSize;
                int y = pathfinder.roads[i].Id / mapSize;
                l2graphics.DrawImage(roadTile.getTile(roadSelect(x, y)), x * tileSize, y * tileSize);
            }
            if (showGrid)
            {
                
                label1.Text = "X ";
                for (int i = 0; i < pathfinder.path.GetLength(0); i++)
                {
                    label1.Text += i + " ";
                }
                label1.Text += "\n";
                for (int i = 0; i < pathfinder.path.GetLength(0); i++)
                {
                    label1.Text += pathfinder.roads[i].Id + " ";
                    for (int j = 0; j < pathfinder.path.GetLength(1); j++)
                    {
                        if (pathfinder.path[i, j] == pathfinder.inf)
                        {
                            label1.Text += "X ";
                        }
                        else
                        {
                            label1.Text += pathfinder.path[i, j] + " ";
                        }
                    }
                    label1.Text += "\n";
                }
            }
            l2graphics.FillEllipse(snake.myBrush, snake.PosX, snake.PosY, tileSize, tileSize);
            
            dynamicBox.Image = layer2;
            inited = true;
        }
        private void keyPressHandler(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 'w')
            {
                snake.direction = (int)GameObject.irany.Up;
            }
            if (e.KeyChar == 's')
            {
                snake.direction = (int)GameObject.irany.Down;
            }
            if (e.KeyChar == 'a')
            {
                snake.direction = (int)GameObject.irany.Left;
            }
            if (e.KeyChar == 'd')
            {
                snake.direction = (int)GameObject.irany.Right;
            }
            if (e.KeyChar == 'g') {
                showGrid = !showGrid;
            }
        }

        private void clickHandler(object sender, MouseEventArgs e)
        {
            label1.Text = (e.X/tileSize).ToString() + " " + (e.Y/tileSize).ToString();
            int x = e.X / tileSize;
            int y = e.Y / tileSize;
            if (x < map.GetLength(0) && y < map.GetLength(1)) {
                if (map[x, y] != 5) {
                    map[x, y] = 5;
                    int id = (y * map.GetLength(0)) + x;
                    pathfinder.run(id);
                }
                else
                {
                    snake.targetX = x;
                    snake.targetY = y;
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
    class GameObject
    {
        private int x;
        private int y;
        int speed = 1;
        public enum irany { Up, Down, Right, Left }
        public int direction = (int)irany.Right;
        public Brush myBrush = Brushes.LightGreen;
        public int targetX = 0;
        public int targetY = 0;
        int roadX = 0;
        int roadY = 0;
        int tileSize;

        public int PosX
        {
            get { return x; }
            set { x = value; }
        }

        public int PosY
        {
            get { return y; }
            set { y = value; }
        }

        public GameObject(int tileSize)
        {
            roadX = 5;
            roadY = 5;
            targetX = 5;
            targetY = 5;
            this.tileSize = tileSize;
        }
        public void run(PathFinder finder)
        {
            
            if (x != targetX*tileSize || y != targetY*tileSize) {
                if (x == roadX*tileSize && y == roadY*tileSize)
                {
                    int id = (roadX + (roadY * Form1.mapSize));
                    int targetId = (targetX + (targetY * Form1.mapSize));
                    int myPlace = finder.HasRoad(id);
                    int targetPlace = finder.HasRoad(targetId);
                    int roadPlace = finder.path[myPlace,targetPlace];
                    if (roadPlace != finder.inf) {
                        roadX = finder.roads[roadPlace].Id % Form1.mapSize;
                        roadY = finder.roads[roadPlace].Id / Form1.mapSize;
                    }
                }
                else {
                    if (x < roadX*tileSize) {
                        x++;//=(tileSize/100f)*speed;
                    }
                    if (x > roadX*tileSize) {
                        x--;//= (tileSize / 100f) * speed;
                    }
                    if (y > roadY*tileSize) {
                        y--;//= (tileSize / 100f) * speed;
                    }
                    if (y< roadY*tileSize) {
                        y ++;//= (tileSize / 100f) * speed;
                    }
                }
            }
        }
    }
    class Road
    {
        public int Id;
        public Road(int id)
        {
            Id = id;
        }
    }
    class Edge
    {
        public int From, To, Distance;
        public Edge(int from, int to, int dis)
        {
            From = from;
            To = to;
            Distance = dis;
        }
    }
    class TileRescure
    {
        Image[,] roundTiles;
        public TileRescure(Image image, int size)
        {
            roundTiles = new Image[1, 4];
            for (int i = 0; i < roundTiles.Length; i++)
            {
                roundTiles[0,i] = ScaleImage(image, size, i);
            }
        }
        public TileRescure(Image[] images, int size)
        {
            roundTiles = new Image[images.Length, 4];
            for (int i = 0; i < images.Length; i++)
            {
                for (int j = 0; j < roundTiles.GetLength(1); j++)
                {
                    roundTiles[i,j] = ScaleImage(images[i], size, j);
                }
            }
        }
        public TileRescure(String roundTilesPath, int size)
        {
            string[] paths = Directory.GetFiles(roundTilesPath,"*.png");
            roundTiles = new Image[paths.Length, 4];
            for (int i = 0; i < paths.Length; i++)
            {
                Image temp = Image.FromFile(paths[i]);
                for (int j = 0; j < roundTiles.GetLength(1); j++)
                {
                    roundTiles[i, j] = ScaleImage(temp, size, j);
                }
            }
        }
        public Image getTile(int i) {
            return roundTiles[0,i];
        }
        public Image getTile(int roundIndex, int num) {
            return roundTiles[roundIndex, num];
        }
        public Image getTile(int[] a) {
            return roundTiles[a[0],a[1]];
        }
        private Image ScaleImage(Image image, int maxSize, int rot)
        {
            var ratioX = (double)maxSize / image.Width;

            var newWidth = (int)(image.Width * ratioX);

            var newImage = new Bitmap(newWidth, newWidth);

            var graphics = Graphics.FromImage(newImage);
            graphics.DrawImage(image, 0, 0, newWidth, newWidth);
            switch (rot)
            {
                case 1:
                    newImage.RotateFlip(RotateFlipType.Rotate90FlipNone);
                    break;
                case 2:
                    newImage.RotateFlip(RotateFlipType.Rotate180FlipNone);
                    break;
                case 3:
                    newImage.RotateFlip(RotateFlipType.Rotate270FlipNone);
                    break;
                default:
                    break;
            }
            return newImage;
        }
    }
    class PathFinder
    {
        public int inf = 999999;
        int[,] map = new int[Form1.mapSize, Form1.mapSize];
        public List<Road> roads = new List<Road>();
        public List<Edge> edgeList = new List<Edge>();
        int[,] dist;
        public int[,] path;
        public void AddRoad(int id)
        {
            if (HasRoad(id) == -1)
            {
                roads.Add(new Road(id));
                int l = map.GetLength(0);
                int x = id % l;
                int y = id / l;
                map[x, y] = 1;
                if (id % l != 0)
                {
                    int leftRoad = HasRoad(id - 1);
                    if (leftRoad != -1)
                    {
                        edgeList.Add(new Edge(roads.Count - 1, leftRoad, 1));
                    }
                }
                if (id / l != 0)
                {
                    int topRoad = HasRoad(id - l);
                    if (topRoad != -1)
                    {
                        edgeList.Add(new Edge(roads.Count - 1, topRoad, 1));
                    }
                }
                if (id / l != l - 1)
                {
                    int downRoad = HasRoad(id + l);
                    if (downRoad != -1)
                    {
                        edgeList.Add(new Edge(roads.Count - 1, downRoad, 1));
                    }
                }
                if (id % l != l - 1)
                {
                    int rightRoad = HasRoad(id + 1);
                    if (rightRoad != -1)
                    {
                        edgeList.Add(new Edge(roads.Count - 1, rightRoad, 1));
                    }
                }
            }
        }
        public int HasRoad(int id)
        {

            for (int i = 0; i < roads.Count; i++)
            {
                if (roads[i].Id == id)
                {
                    return i;
                }
            }
            return -1;
        }

        public void run(int id)
        {       AddRoad(id);
                runFloydWarshall(ref dist, ref path);   
        }
        void runFloydWarshall(ref int[,] dist, ref int[,] path)
        {
            int l = roads.Count;
            dist = new int[l, l];
            path = new int[l, l];
            for (int i = 0; i < dist.GetLength(0); i++)
            {
                for (int j = 0; j < dist.GetLength(1); j++)
                {
                    dist[i, j] = inf;
                    path[i, j] = inf;
                }
            }
            for (int i = 0; i < l; i++)
            {
                dist[i, i] = 0;
            }
            foreach (Edge edge in edgeList)
            {
                dist[edge.From, edge.To] = edge.Distance;
                dist[edge.To, edge.From] = edge.Distance;
                path[edge.From, edge.To] = edge.To;
                path[edge.To, edge.From] = edge.From;
            }
            for (int k = 0; k < l; k++)
            {
                for (int i = 0; i < l; i++)
                {
                    for (int j = 0; j < l; j++)
                    {
                        if (dist[i, j] > dist[i, k] + dist[k, j])
                        {
                            dist[i, j] = dist[i, k] + dist[k, j];
                            path[i, j] = path[i, k];
                        }
                    }
                }
            }
        }
        
    } 
}
