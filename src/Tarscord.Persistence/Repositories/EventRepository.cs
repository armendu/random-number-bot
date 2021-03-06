﻿using Tarscord.Persistence.Entities;
using Tarscord.Persistence.Interfaces;

namespace Tarscord.Persistence.Repositories
{
    public class EventRepository : BaseRepository<EventInfo>, IEventRepository
    {
        public EventRepository(IDatabaseConnection connection)
            : base(connection)
        {
        }
    }
}