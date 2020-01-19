﻿using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;

namespace NeoFx.Models
{
    [Obsolete]
    public sealed class EnrollmentTransaction : Transaction
    {
        public readonly EncodedPublicKey PublicKey;

        public EnrollmentTransaction(EncodedPublicKey publicKey,
                                     byte version,
                                     ImmutableArray<TransactionAttribute> attributes,
                                     ImmutableArray<CoinReference> inputs,
                                     ImmutableArray<TransactionOutput> outputs,
                                     ImmutableArray<Witness> witnesses)
            : base(version, attributes, inputs, outputs, witnesses)
        {
            PublicKey = publicKey;
        }
    }
}
