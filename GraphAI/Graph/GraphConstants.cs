// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

// <GraphConstantsSnippet>
namespace GraphAI
{
    public static class GraphConstants
    {
        // Defines the permission scopes used by the app
        public readonly static string[] Scopes =
        {
            "User.Read",
            "MailboxSettings.Read",
            "Calendars.ReadWrite",
            "Tasks.Read",
            "Tasks.ReadWrite"
        };
    }
}
// </GraphConstantsSnippet>
