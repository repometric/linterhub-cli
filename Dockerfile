FROM microsoft/dotnet:1.1.0-preview1-runtime
MAINTAINER Repometric <docker@repometric.com>

# Configure bash
RUN echo "dash dash/sh boolean false" | debconf-set-selections
RUN DEBIAN_FRONTEND=noninteractive dpkg-reconfigure dash
RUN find . -name "*.sh" -exec chmod +x {} \;

# Install curl
RUN apt-get update
RUN apt-get --assume-yes install curl 

# Install docker
ENV DOCKER_BUCKET get.docker.com
ENV DOCKER_VERSION 1.12.3
ENV DOCKER_SHA256 626601deb41d9706ac98da23f673af6c0d4631c4d194a677a9a1a07d7219fa0f
RUN set -x \
	&& curl -fSL "https://${DOCKER_BUCKET}/builds/Linux/x86_64/docker-${DOCKER_VERSION}.tgz" -o docker.tgz \
	&& echo "${DOCKER_SHA256} *docker.tgz" | sha256sum -c - \
	&& tar -xzvf docker.tgz \
	&& mv docker/* /usr/local/bin/ \
	&& rmdir docker \
	&& rm docker.tgz \
	&& docker -v

WORKDIR /temp
ADD linterhub-cli-debian.8-x64.zip ./
ADD src/cli/Config bin/debian.8-x64/Config
ADD src/cli/linterhub bin/debian.8-x64/linterhub
WORKDIR /app/bin
RUN mv /temp/bin/debian.8-x64/* /app/bin && \
    rm -rf /temp