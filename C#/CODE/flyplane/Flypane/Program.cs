using System;

namespace Flypane
{
    /*□◎★▲卍*/
    class Program
    {
        /// <summary>
        /// 0,1,2,3,4
        /// 
        /// </summary>
        static int[] Maps = new int[100];
        static int[] player = new int[2];
        static string[] PlayerName = new string[2];
        static int[] playersite = new int[2];//玩家位置

        static void Main(string[] args)
        {
            GameHead();//头
            GameMapInit();//数字填充

            MapDraw();//画地图
            while (playersite[0] < 99 && playersite[1] < 99)
            {
                PlayGame(0);
                PlayGame(1);
            }

        }
        public static void ChangePots()
        {
            if (playersite[0] < 0)
                playersite[0] = 0;
            else if(playersite[0]>=99)
            {
                playersite[0] = 99;
            }

            if (playersite[1] < 0)
                playersite[1] = 0;
            else if (playersite[1] >= 99)
            {
                playersite[1] = 99;
            }
        }
        public static void PlayGame(int playernumber)
        {
            Random random = new Random();
            int r1 = random.Next(6);

                Console.WriteLine("{0}按任意键开始投色子：", PlayerName[playernumber]);
                Console.ReadKey(true);
                Console.WriteLine("{0}掷出了{1}：", PlayerName[playernumber],r1);
                playersite[playernumber] += r1;
                Console.ReadKey(true);
                Console.WriteLine("{0}按任意键开始行动：", PlayerName[playernumber]);
                Console.ReadKey(true);
                Console.WriteLine("{0}行动完了：", PlayerName[playernumber]);
                Console.ReadKey(true);
                if (playersite[playernumber] == playersite[1- playernumber])
                {
                    Console.WriteLine("{0}玩家踩到玩家{1}，玩家{2}退6格", PlayerName[playernumber], PlayerName[1- playernumber]);
                    playersite[1- playernumber] -= 6;
                    Console.ReadKey(true);
                }
                else//踩到关卡
                {
                    switch (Maps[playersite[playernumber]])
                    {
                        case 0:
                            Console.WriteLine("玩家踩到方块，什么都不发生");
                            Console.ReadKey(true);
                            break;
                        case 1:
                            Console.WriteLine("玩家踩到幸运方块，1--交换位置，2--轰炸对方");
                            string input = Console.ReadLine();
                            while (true)
                            {
                                if (input == "1")
                                {
                                    Console.WriteLine("玩家选择交换位置");
                                    int temp;
                                    temp = playersite[playernumber];
                                    playersite[playernumber] = playersite[1- playernumber];
                                    playersite[1- playernumber] = temp;
                                    Console.WriteLine("交换完成");
                                    break;
                                }
                                else if (input == "2")
                                {
                                    Console.WriteLine("玩家选择轰炸");
                                    playersite[1- playernumber] -= 6;
                                    break;
                                }
                                else
                                {
                                    Console.WriteLine("重新输入");
                                }
                            }
                            break;
                        case 2:
                            Console.WriteLine("玩家{0}踩到地雷，退6格", playersite[playernumber]);
                            playersite[playernumber] -= 6;
                            break;
                        case 3:
                            Console.WriteLine("玩家{0}踩到暂停，暂停一回合", playersite[playernumber]);

                            break;
                        case 4:
                            break;
                    }
                }
            
        }
        public static void InputpPlayerName()
        {
            a:
            Console.Write("Input player A name: ");

            PlayerName[0] =  Console.ReadLine();
            while (PlayerName[0] == "")
            {
                Console.WriteLine("Name not NULL");
                PlayerName[0] = Console.ReadLine();
            }
            Console.Write("Input player B name: ");
            PlayerName[1] = Console.ReadLine();
            while (PlayerName[1] == "")
            {
                Console.WriteLine("Name not NULL");
                PlayerName[1] = Console.ReadLine();
            }
            if(PlayerName[0]==PlayerName[1])
            {
                Console.WriteLine("name is not ok");
                goto a;
            }
        }

        public static void MapDraw()
        {
            //第一行
            for(int i = 0;i<30;i++)
            {
                Console.Write(DrawStringMap(i));
            }
            Console.WriteLine();
            //第一竖行
            for(int i=30;i<35;i++)
            {
                for(int j=0;j<=28;j++)
                    Console.Write("  ");
                Console.WriteLine(DrawStringMap(i));
            }
            //第二横行 64->35
            for(int i=64;i>=35;i--)
            {
                Console.Write(DrawStringMap(i));
            }
            Console.WriteLine();

            //第二竖行
            for (int i = 65; i < 70; i++)
            {
                Console.WriteLine(DrawStringMap(i));

            }
                //第三横行
            for (int i = 70; i < 100; i++)
                Console.Write(DrawStringMap(i));

        }

        public static string DrawStringMap(int i)
        {
            string str = "";
                if (player[0] == player[1] && player[0] == i)
                    str = "<>";
                else if (player[0] == i)
                    str = "Ａ";
                else if (player[1] == i)
                    str = "Ｂ";
                else
                {
                    switch (Maps[i])
                    {
                        case 0:
                           str = "□";
                            break;
                        case 1:
                           str = "◎";
                            break;
                        case 2:
                           str = "★";
                            break;
                        case 3:
                            str = "▲";
                            break;
                        case 4:
                            str = "卍";
                            break;

                    }
                }
            return str;
            
        }

        public static void GameMapInit()
        {
            int[] Lucky = { 6, 23, 40, 55, 69, 83 };
            for(int i =0;i<Lucky.Length;i++)
                Maps[Lucky[i]] = 1;

            int[] LandMine = { 5, 13, 17, 33, 50, 64, 94 };//地雷
            for (int i = 0; i < LandMine.Length; i++)
                Maps[LandMine[i]] = 2;
            int[] pause = { 9, 27, 60, 93 };//暂停
            for (int i = 0; i < pause.Length; i++)
                Maps[pause[i]] = 2;
            int[] timeTunnel = { 20, 25, 45, 63, 72, 90 };//时空隧道
            for (int i = 0; i < timeTunnel.Length; i++)
                Maps[timeTunnel[i]] = 2;
        }

        public static void GameHead()
        {
            Console.WriteLine("********************");
            Console.WriteLine("******FlyPlane******");
            Console.WriteLine("********************");
        }
    }
}
