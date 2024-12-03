# Dockerfile for writing and running solutions to the Advent Of Code 2024
# in C#/Dotnet 9, Python 3 and Rust. Used in conjunction with the
# associated devcontainer.json
FROM mcr.microsoft.com/devcontainers/dotnet:9.0

# Set environment variables to non-interactive
ENV DEBIAN_FRONTEND=noninteractive

COPY ./bash_aliases /root/.bash_aliases
COPY ./bash_aliases $HOME/.bash_aliases

# Install dependencies & useful tools
RUN apt update && apt install -y --no-install-recommends \
    apt-transport-https software-properties-common \
    curl wget git build-essential ca-certificates \
    mc htop ripgrep fd-find dnsutils iproute2 jq zsh \
    && rm -rf /var/lib/apt/lists/*

# Install Python
RUN apt-get update && apt-get install -y --no-install-recommends \
    python3 \
    python3-pip \
    python3-venv \
    && rm -rf /var/lib/apt/lists/*

# Rust is installed from within the devcontainer.json

# Dotnet workload
RUN dotnet workload update

# if using a tool that doesn't support devcontainers then you can use the devcontainer CLI
# at https://github.com/devcontainers/cli
# Install it using
#    $> npm install -g @devcontainers/cli
# From the project root directory run
#    $> devcontainer up --workspace-folder .