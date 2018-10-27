﻿using System.Collections.Generic;
using System.Threading.Tasks;

namespace MonkeyBot.Services
{
    public interface IFeedService
    {
        /// <summary>
        /// Start the feed service to regularly check for updates
        /// </summary>
        void Start();

        /// <summary>
        /// Add a new feed url. Updates will be posted in the provided channel
        /// </summary>
        /// <param name="url">URL of the feed (atom/rss)</param>
        /// <param name="guildId"></param>
        /// <param name="channelId"></param>
        /// <returns></returns>
        Task AddFeedAsync(string url, ulong guildId, ulong channelId);

        /// <summary>
        /// Removes the specified feed url from the channel
        /// </summary>
        /// <param name="url">URL of the feed</param>
        /// <param name="guildId"></param>
        /// <param name="channelId"></param>
        /// <returns></returns>
        Task RemoveFeedAsync(string url, ulong guildId, ulong channelId);

        /// <summary>
        /// Removes all feeds. If a channel is provided only the feeds in this channel are removed
        /// </summary>
        /// <param name="guildId"></param>
        /// <param name="channelId"></param>
        /// <returns></returns>
        Task RemoveAllFeedsAsync(ulong guildId, ulong? channelId);

        /// <summary>
        /// Returns all subscribed feeds. If a channel is provided only feeds in this channel are returned.
        /// </summary>
        /// <param name="guildId"></param>
        /// <param name="channelId"></param>
        /// <returns></returns>
        Task<List<(ulong feedChannelId, string feedUrl)>> GetFeedUrlsForGuildAsync(ulong guildId, ulong? channelId = null);
    }
}