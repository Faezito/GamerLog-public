# 🎮 GamerLog

![C#](https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=csharp&logoColor=white)
![.NET](https://img.shields.io/badge/.NET-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![SQL Server](https://img.shields.io/badge/SQL_Server-CC2927?style=for-the-badge&logo=microsoftsqlserver&logoColor=white)
![JavaScript](https://img.shields.io/badge/JavaScript-F7DF1E?style=for-the-badge&logo=javascript&logoColor=black)
![Status](https://img.shields.io/badge/status-online-brightgreen?style=for-the-badge)

> Plataforma web para registrar e acompanhar suas jogatinas — com dados automáticos dos jogos via RAWG API e enriquecimento por inteligência artificial com Google Gemini.

🔗 **[Acessar o GamerLog](https://gamerlog.runasp.net/)**

---

## 📋 Sobre o projeto

O **GamerLog** é uma plataforma fullstack onde jogadores podem criar uma conta, fazer login e registrar seus históricos de jogatinas de forma simples e organizada.

O diferencial do projeto está na automação do cadastro de jogos: ao registrar uma jogatina, os dados do jogo são buscados automaticamente na **RAWG API** e persistidos no banco de dados. Informações complementares são enriquecidas pelo **Google Gemini**, eliminando a necessidade de preenchimento manual.

---

## ✨ Funcionalidades

- 👤 **Autenticação** — cadastro e login de usuários seguro com BCrypt
- 🎮 **Registro de jogatinas** — registre seus jogos com status, nota e comentários
- 🔍 **Busca automática de jogos** — dados buscados via RAWG API no cadastro
- 🤖 **Enriquecimento com IA** — informações complementares via Google Gemini
- 💾 **Persistência local** — jogos salvos no banco para consultas rápidas futuras
- 📱 **Interface web responsiva** — acesso direto pelo navegador

---

## 🏗️ Arquitetura

O projeto segue uma **arquitetura em camadas** organizada por namespaces prefixados:

```
Z1.Model        →  Entidades e modelos de domínio
Z2.Services     →  Regras de negócio e integrações externas (RAWG, Gemini)
Z3.DataAccess   →  Repositórios e acesso ao SQL Server
Z4.Lib          →  Utilitários e helpers compartilhados
GameDB-v3       →  Scripts e configurações do banco de dados
```

```
Fluxo de cadastro de jogatina:
Usuário → Z2.Services → RAWG API (dados do jogo)
                      → Google Gemini (dados complementares)
                      → Z3.DataAccess → SQL Server
```

---

## 🛠️ Tecnologias utilizadas

| Tecnologia | Uso |
|-----------|-----|
| C# / .NET | Back-end e lógica de negócio |
| SQL Server | Banco de dados relacional |
| JavaScript / HTML / CSS | Front-end web |
| RAWG API | Catálogo de jogos |
| Google Gemini | Enriquecimento de dados com IA |
| runasp.net | Hospedagem em nuvem |

---

## 🗺️ Roadmap

- [ ] **Dashboard de estatísticas** — gêneros mais jogados, tempo estimado, plataformas favoritas e muito mais
- [ ] **Rede social integrada** — compartilhe opiniões, dicas, truques e conquistas com outros jogadores
- [ ] **Sistema de comentários** — interaja com os registros de jogo da comunidade

---

## 👨‍💻 Autor

Feito por **[Faezito](https://github.com/Faezito)**

[![LinkedIn](https://img.shields.io/badge/LinkedIn-0077B5?style=for-the-badge&logo=linkedin&logoColor=white)](https://linkedin.com/in/seu-perfil)
[![GitHub](https://img.shields.io/badge/GitHub-100000?style=for-the-badge&logo=github&logoColor=white)](https://github.com/Faezito)
