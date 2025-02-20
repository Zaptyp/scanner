﻿﻿using Microsoft.Toolkit.Mvvm.Messaging.Messages;

namespace Scanner.Services.Messenger
{
    class EditorCurrentIndexChangedMessage : ValueChangedMessage<int>
    {
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // DECLARATIONS /////////////////////////////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        int NewIndex;

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // CONSTRUCTORS / FACTORIES /////////////////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public EditorCurrentIndexChangedMessage(int index) : base(index)
        {
            NewIndex = index;
        }
    }
}