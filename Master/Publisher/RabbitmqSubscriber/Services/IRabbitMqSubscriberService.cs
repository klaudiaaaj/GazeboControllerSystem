﻿using Contracts.Models;

namespace RabbitmqSubscriber.Services
{
    public interface IRabbitMqSubscriberService
    {
        Task<Joystic> ExecuteAsyncSingle();
    }
}
