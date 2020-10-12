﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quras
{
    public enum AssetType : int
    {
        CreditFlag = 0x40,
        DutyFlag = 0x80,

        GoverningToken = 0x00,
        UtilityToken = 0x01,
        Currency = 0x08,
        Share = DutyFlag | 0x10,
        Invoice = DutyFlag | 0x18,
        Token = CreditFlag | 0x20,
        AnonymousToken = CreditFlag | 0x21,
        TransparentToken = CreditFlag | 0x22,
        StealthToken = CreditFlag | 0x23
    }
}
