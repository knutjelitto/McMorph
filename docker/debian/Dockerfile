FROM microsoft/dotnet:2.1.300-sdk

RUN apt-get update -y && apt-get install -y \
    apt-transport-https \
    xz-utils \
    less \
    tree \
    man \
    manpages \
    dirmngr \
    bzr \
    lzip \
    autoconf \
    gawk \
    attr \
    nano \
    build-essential \
 && apt-get -q -y autoremove  && rm -rvf /var/lib/apt/lists/*

ENV TERM=xterm
ENV PATH=/sbin:/bin:/usr/sbin:/usr/bin:/root/LiFo/bin

RUN rm -rf /root/.[a-z]*
WORKDIR /root

CMD [ "bash",  "-l" ]
