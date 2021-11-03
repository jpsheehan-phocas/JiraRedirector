# Jira Redirector

Adds a new protocol handler so that you can link to your Jira tickets like: `jira:abc-12345`.

## Installation

1. Build the project as "Release".
2. Copy the files to a new directory called `C:\tools\JiraRedirector\` (you may change the location but will have to alter the `.reg` file used in the next step).
3. Run the `register_jira_protocol.reg` file as an administrator to register the protocol.
4. Next, create a text file at `%APPDATA%/JiraRedirector.settings` that contains the base URL of your Jira instance. For example, the file may contain `https://jira.example.com/browse/`.
5. (Optional) Copy the `git-prompt.sh` file to your `.config/git/git-prompt.sh` if you want a Jira link to appear in your Git Bash.
