﻿using MonkeyBot.Database.Repositories;
using System;
using System.Threading.Tasks;

namespace MonkeyBot.Database
{
    public class UnitOfWork : IUnitOfWork
    {
        private MonkeyDBContext context { get; }

        private IGuildConfigRepository guildConfigs;
        public IGuildConfigRepository GuildConfigs => guildConfigs ?? (guildConfigs = new GuildConfigRepository(context));

        private IAnnouncementRepository announcements;
        public IAnnouncementRepository Announcements => announcements ?? (announcements = new AnnouncementRepository(context));

        private ITriviaScoresRepository triviaScores;
        public ITriviaScoresRepository TriviaScores => triviaScores ?? (triviaScores = new TriviaScoresRepository(context));

        private IBenzenFactsRespository benzenFacts;
        public IBenzenFactsRespository BenzenFacts => benzenFacts ?? (benzenFacts = new BenzenFactsRespository(context));

        private IGameServersRepository gameServers;
        public IGameServersRepository GameServers => gameServers ?? (gameServers = new GameServersRepository(context));

        public UnitOfWork(MonkeyDBContext context)
        {
            this.context = context;
        }

        public Task<int> CompleteAsync()
        {
            return context.SaveChangesAsync();
        }

        private bool disposed = false;

        protected void Dispose(bool disposing)
        {
            if (!this.disposed)
                if (disposing)
                    context.Dispose();
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}