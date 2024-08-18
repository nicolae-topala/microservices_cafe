﻿using System.ComponentModel;
using System.Runtime.Serialization;

namespace MicroservicesCafe.Shared.Enums;

public enum CurrencyEnum
{
    [EnumMember(Value = "EUR")]
    [Description("Euro")]
    EUR,

    [EnumMember(Value = "USD")]
    [Description("United States Dollar")]
    USD,
}