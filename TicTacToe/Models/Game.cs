using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Globalization;
using System.Web.Security;
using System.Web.Caching;

namespace TicTacToe.Models
{
    [Serializable]
    public class Game
    {
        public Game()
        {
            Data = new string[][] { 
                new string[] { "", "", "" }, 
                new string[] { "", "", "" }, 
                new string[] { "", "", "" } 
            };
        }
        
        public  User First { set; get; }
        public User Second { set; get; }
        public string Key { set; get; }
        public string Next { set; get; }
        public string[][] Data { set; get; }

        public void assignPlayer(User player)
        {
            if (First == null)
            {
                First = player;
                player.Marker = "X";
            }
            else if (Second == null && First.SessionID != player.SessionID)
            {
                Second = player;
                player.Marker = "O";
            }
            else if (First.SessionID == player.SessionID)
            {
                player.Marker = "X";
            }
            else if (Second.SessionID == player.SessionID)
            {
                player.Marker = "O";
            }
        }

        public bool isMarked(int x, int y)
        {
            return !string.IsNullOrEmpty(Data[x][y]);
        }

        public bool isValid(int x, int y)
        {
            return (x >= 0 && x < 3) && (y >= 0 && y < 3);
        }

        public void mark(int x, int y, User player)
        {
            Data[x][y] = player.Marker;
            Next = Next == "X" ? "O" : "X";
        }

        public bool isCurrentPlay(User player)
        {
            return player.Marker == Next;            
        }

        public string isWin() 
        {
            for (var i = 0; i < 3; i++)
            {
                // horizontal
                if (!string.IsNullOrEmpty(Data[0][i]) && Data[0][i] == Data[1][i] && Data[1][i] == Data[2][i]) return Data[0][i];

                // vertical
                if (!string.IsNullOrEmpty(Data[i][0]) && Data[i][0] == Data[i][1] && Data[i][1] == Data[i][2]) return Data[i][0];
            }

            // forward slash
            if (!string.IsNullOrEmpty(Data[1][1]) && Data[0][0] == Data[1][1] && Data[1][1] == Data[2][2]) return Data[1][1];

            // back slash
            if (!string.IsNullOrEmpty(Data[1][1]) && Data[2][0] == Data[1][1] && Data[1][1] == Data[0][2]) return Data[1][1];

            return "";
        }
    }

    [Serializable]
    public class User
    {
        public User()
        {

        }

        public string Name { set; get; }
        public string SessionID { set; get; }
        public string Marker { set; get; }
    }

    public class DB
    {
        public DB()
        {
        }

        public static string SessionID { get { return System.Web.HttpContext.Current.Session.SessionID; } }

        public static User User(string key) {            
            var user = System.Web.HttpContext.Current.Cache["user" + key] as User;
            return user;
        }
              
        public static User User(string key, User user) {  
            System.Web.HttpContext.Current.Cache["user" + key]  = user;
            return user;            
        }

        public static Game Games(string key)
        {
            var game = System.Web.HttpContext.Current.Cache["games" + key] as Game;
            return game;
        }

        public static Game Games(string key, Game game)
        {
            System.Web.HttpContext.Current.Cache["games" + key] = game;
            return game;
        }
    }
}
