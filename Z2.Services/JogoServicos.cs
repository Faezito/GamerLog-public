using GenerativeAI.Types;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Z1.Model;
using Z1.Model.APIs;
using Z2.Services.Externo;
using Z3.DataAccess;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Z2.Services
{
    public interface IJogoServicos
    {
        Task<List<JogoModel>> Listar(int? id, string? titulo);
        Task<List<RegistroJogoModel>> ListarJogosDoUsuario(int? id, string? titulo, int? status, int usuarioID, int? ano = null, int? mes = null);
        Task<int> Cadastro(int id, JogoModel jogo);
        Task<JogoModel> Obter(int id);
        Task<int> Inserir(int id, JogoModel jogo);
        Task<int> Atualizar(int id, JogoModel jogo);
    }

    public class JogoServicos : IJogoServicos
    {
        private readonly IJogoDataAccess _daJogo;
        private readonly RawgService _rawg;
        private readonly IGeminiServicos _gemini;
        private readonly IRegistroJogoServicos _registro;

        public JogoServicos(RawgService rawg, IGeminiServicos gemini, IRegistroJogoServicos registro, IJogoDataAccess daJogo)
        {
            _rawg = rawg;
            _gemini = gemini;
            _registro = registro;
            _daJogo = daJogo;
        }

        public async Task<int> Cadastro(int id, JogoModel jogo)
        {
            RawgGameDto rawgJogo = new();

            try
            {
                rawgJogo = await _rawg.ObterJogoPorID(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            if (jogo == null)
            {
                try
                {
                    string GeneroEPublisher = await ObterGeneroEPublisher(rawgJogo.Name);
                    string genero = GeneroEPublisher.Split(",")[0].Trim();
                    string publisher = GeneroEPublisher.Split(",")[1].Trim();

                    int generoID = int.TryParse(genero, out int generoValor) ? generoValor : 0;
                    int publisherID = int.TryParse(publisher, out int publisherValor) ? publisherValor : 0;

                    jogo = new JogoModel
                    {
                        ID = rawgJogo.Id,
                        Titulo = rawgJogo.Name,
                        DataLancamento = rawgJogo.Released,
                        Metacritic = rawgJogo.Metacritic,
                        CaminhoImagem = rawgJogo.Background_Image,
                        GeneroID = generoID,
                        PublisherID = publisherID
                    };

                    return await _daJogo.Adicionar(jogo);
                }
                catch (Exception)
                {
                    throw;
                }
            }

            return await _daJogo.Atualizar(jogo);
        }

        public async Task<int> Inserir(int id, JogoModel jogo)
        {
            RawgGameDto rawgJogo = new();
            rawgJogo = await _rawg.ObterJogoPorID(id);

            try
            {
                string GeneroEPublisher = await ObterGeneroEPublisher(rawgJogo.Name);
                string genero = GeneroEPublisher.Split(",")[0].Trim();
                string publisher = GeneroEPublisher.Split(",")[1].Trim();

                int generoID = int.TryParse(genero, out int generoValor) ? generoValor : 0;
                int publisherID = int.TryParse(publisher, out int publisherValor) ? publisherValor : 0;

                jogo = new JogoModel
                {
                    ID = rawgJogo.Id,
                    Titulo = rawgJogo.Name,
                    DataLancamento = rawgJogo.Released,
                    Metacritic = rawgJogo.Metacritic,
                    CaminhoImagem = rawgJogo.Background_Image,
                    GeneroID = generoID,
                    PublisherID = publisherID
                };

                return await _daJogo.Adicionar(jogo);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<int> Atualizar(int id, JogoModel jogo)
        {
            try
            {
                string GeneroEPublisher = await ObterGeneroEPublisher(jogo.Titulo);
                string genero = GeneroEPublisher.Split(",")[0].Trim();
                string publisher = GeneroEPublisher.Split(",")[1].Trim();

                int generoID = int.TryParse(genero, out int generoValor) ? generoValor : 0;
                int publisherID = int.TryParse(publisher, out int publisherValor) ? publisherValor : 0;

                jogo = new JogoModel
                {
                    ID = jogo.ID,
                    GeneroID = generoID,
                    PublisherID = publisherID
                };

                return await _daJogo.Atualizar(jogo);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<JogoModel>> Listar(int? id, string? titulo)
        {
            var lst = await _daJogo.Listar(id, titulo);
            return lst;
        }

        public async Task<List<RegistroJogoModel>> ListarJogosDoUsuario(int? id, string? titulo, int? status, int usuarioID, int? ano = null, int? mes = null)
        {
            var lst = await _daJogo.ListarJogosDoUsuario(id, titulo, status, usuarioID, ano, mes);
            return lst;
        }

        public async Task<JogoModel> Obter(int id)
        {
            return await _daJogo.Obter(id);
        }


        private async Task<string> ObterGeneroEPublisher(string titulo)
        {
            string res = string.Empty;

            string prompt = @$"
Considerando:
Publishers: 1 - Nintendo, 2 - Sony Interactive Entertainment, 3 - Microsoft Studios, 4 - Xbox Game Studios, 5 - Bethesda Softworks, 6 - Activision, 7 - Activision Blizzard, 8 - Blizzard Entertainment, 9 - Electronic Arts, 10 - EA Sports, 11 - Ubisoft, 12 - Take-Two Interactive, 13 - Rockstar Games, 14 - 2K Games, 15 - Square Enix, 16 - Capcom, 17 - Bandai Namco Entertainment, 18 - Sega, 19 - Konami, 20 - Warner Bros. Games, 21 - Valve, 22 - CD Projekt, 23 - CD Projekt Red, 24 - Epic Games, 25 - THQ Nordic, 26 - Embracer Group, 27 - Focus Entertainment, 28 - Paradox Interactive, 29 - Devolver Digital, 30 - Team17, 31 - Annapurna Interactive, 32 - Private Division, 33 - 505 Games, 34 - Deep Silver, 35 - Nacon, 36 - SNK, 37 - Atlus, 38 - Koei Tecmo, 39 - FromSoftware, 40 - Larian Studios, 41 - Remedy Entertainment, 42 - Obsidian Entertainment, 43 - Supergiant Games, 44 - Playdead, 45 - Hello Games, 46 - Mojang Studios, 47 - Riot Games, 48 - Behaviour Interactive, 49 - Telltale Games, 50 - Gearbox Publishing, 51 - Pearl Abyss, 52 - NCSoft, 53 - Nexon, 54 - NetEase Games, 55 - Tencent Games
Gêneros: 1 - Action, 2 - Adventure, 3 - Action-Adventure, 4 - Role-Playing, 5 - RPG, 6 - JRPG, 7 - ARPG, 8 - Strategy, 9 - Turn-Based Strategy, 10 - Real-Time Strategy, 11 - Tactical, 12 - Simulation, 13 - Management, 14 - Tycoon, 15 - Sports, 16 - Racing, 17 - Fighting, 18 - Shooter, 19 - First-Person Shooter, 20 - Third-Person Shooter, 21 - Platformer, 22 - Puzzle, 23 - Stealth, 24 - Survival, 25 - Survival Horror, 26 - Horror, 27 - Visual Novel, 28 - Interactive Fiction, 29 - Sandbox, 30 - Open World, 31 - MMO, 32 - MMORPG, 33 - Roguelike, 34 - Roguelite, 35 - Card Game, 36 - Deckbuilding, 37 - Rhythm, 38 - Music, 39 - Party, 40 - Casual, 41 - Educational, 42 - Family, 43 - Indie, 44 - Point-and-Click, 45 - Beat'em up, 46 - Hack and Slash, 47 - Metroidvania, 48 - Idle, 49 - Clicker, 50 - Tower Defense, 51 - MOBA, 52 - Battle Royale, 53 - Auto Battler, 54 - Twin-Stick Shooter, 55 - Bullet Hell, 56 - Shoot \`em up, 57 - Text-Based, 58 - Narrative, 59 - Story Rich, 60 - Exploration, 61 - Dating Sim, 62 - Otome, 63 - Cooking, 64 - Farming, 65 - City Builder, 66 - Construction, 67 - Physics-Based, 68 - Time Management, 69 - Escape Room

Me responda, de forma curta e direta, apenas com a resposta solicitada, em português brasileiro, no formato (resposta 1 ID, resposta 2 ID).

Qual é o gênero e publisher do jogo {titulo}?";

            int idGemini = 1;

            string[] errosGemini = ["Erro:429", "Erro:503"];
            res = await _gemini.Prompt(prompt, idGemini);
            while (errosGemini.Contains(res) && idGemini < 4)
            {
                idGemini += 1;
                res = await _gemini.Prompt(prompt, idGemini);
            }
            if (errosGemini.Contains(res))
            {
                res = "(0,0)";
                //throw new Exception("Sistema sobrecarregado, tente novamente mais tarde.");
            }

            res = res.Replace("(", "").Replace(")", "");
            return res;
        }
    }
}
