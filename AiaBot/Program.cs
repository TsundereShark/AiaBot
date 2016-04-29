using DiscordSharp;
using DiscordSharp.Objects;
using Discord.Net;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AiaBot
{
    class Program
    {
        public static DiscordSharp.Objects.DiscordChannel lastchannel;
        static void Main(string[] args)
        {
            // Connecter au Channel
            Console.WriteLine("Définir les variables");
            DiscordClient client = new DiscordClient();
            client.ClientPrivateInformation.Email = "email compte discord";
            client.ClientPrivateInformation.Password = "pass compte discord";

            // Set les events avant de se connecter au discord.
            Console.WriteLine("Définir les Events");
            // Trouver le chan

            client.Connected += (sender, e) => // Le Client est connecté à Discord
            {
                Console.WriteLine("Connecter! Utilisateur: " + e.User.Username);
                // Si le bot est bel et bien connecté, ce message va apparaître
                // Change le "Joue à:"
                client.UpdateCurrentGame("YANDERE SIMULATOR");
            };


            client.PrivateMessageReceived += (sender, e) => // Message privé reçu
            {
                if (e.Message == "!aide")
                {
                    e.Author.SendMessage("Salut! J'ai vu que tu demandais de l'aide. Voici la liste de mes commandes!");
                    e.Author.SendMessage("!aide: La liste de mes commandes.");
                    e.Author.SendMessage("Aïa, out!: Pour me mettre hors ligne.");
                    // Aïa devrait renvoyé le message en privé
                }
                if (e.Message.StartsWith("join ")) //Si quelqu'un d'autre invite Aïa sur un autre Channel (j'espère pas)
                {
                    string inviteID = e.Message.Substring(e.Message.LastIndexOf('/') + 1);
                    client.AcceptInvite(inviteID);
                    e.Author.SendMessage("J'ai join ton Channel");
                    Console.WriteLine("On m'a invité là: " + inviteID); //Me dire le channel dans la console
                }
            };


            client.MessageReceived += (sender, e) => // Message de channel reçu
            {
                if (e.MessageText == "Aïa, out!")
                {
                    bool ismj = false; //ismj est toujours faux à la base
                    List<DiscordRole> roles = e.Author.Roles; //Faire une liste des rôles sur le serveur
                    foreach (DiscordRole role in roles) //Pour chaque rôle dans la liste
                    {
                        if (role.Name.Contains("MJ")) //Si c'est MJ, mettre isMJ à vrai
                        {
                            ismj = true;
                        }
                    }
                    if (ismj) //Dire bye bye
                    {
                        e.Channel.SendMessage("Au revoir!");
                        Environment.Exit(1);
                    }
                    else //Sinon envoyer chier
                    {
                        e.Channel.SendMessage("Ne me dis pas quoi faire!");
                    }
                }
                // Réponses
                if (e.MessageText == "!aide")
                {
                    e.Channel.SendMessage("Envoie moi un message privé!");
                }
                if (e.MessageText == "DAMMIT BRIM")
                {
                    e.Channel.SendMessage("DAMMIT BRIM "); //L'espace après est important sinon elle se répète et crash :(
                }
                if (e.MessageText == "!loli")
                {
                    e.Channel.SendMessage("PEDO ALERT!");
                }
                if (e.MessageText == "!règles")
                {
                    e.Channel.SendMessage("Bienvenue sur le Discord! Il y a quelques règles...");
                    e.Channel.SendMessage("1. Soyez gentil");
                    e.Channel.SendMessage("2. Tout est de la faute de Brimstone");
                }
                if (e.MessageText == "!yaoi")
                {
                    e.Channel.SendMessage("Ooooh j'aime!");
                    e.Channel.SendMessage("https://www.youtube.com/watch?v=Gfl-CfEQcew");
                }

            };


            //¸Quand on nouveau membre join le chan
            client.UserAddedToServer += (sender, e) =>
            {
                e.AddedMember.SendMessage("Bienvenue sur le Discord! Il y a quelques règles...");
                e.AddedMember.SendMessage("1. Soyez gentil");
                e.AddedMember.SendMessage("2. Tout est de la faute de Brimstone");
            };

            // Si quelque chose ne va pas quand le bot essaie de se connecter
            try
            {
                Console.WriteLine("Envoie Requête de Connection");
                client.SendLoginRequest();
                Console.WriteLine("Connecter le clien dans une autre instance");
                Thread connect = new Thread(client.Connect);
                connect.Start();
                // Requête de connection et puis connecté le bot sur le client créé
                Console.WriteLine("Client connecté! Yay!");
            }
            catch (Exception e)
            {
                Console.WriteLine("Y'a quelque chose qui n'a pas passé\n" + e.Message + "\nPèse sur n'importe quelle touche pour fermer.");
            }

            // Être certaine que la console ne ferme pas
            Console.ReadKey(); // Ferme quand on touche à une touche
            Environment.Exit(0); // Être certain que toutes les instances fonctionnent.
        }
    }
}

