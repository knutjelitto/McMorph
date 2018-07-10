FROM alpine:3.8

RUN apk update && apk upgrade && apk add --no-cache \
    bash binutils coreutils diffutils findutils gcc g++ make libc-dev fortify-headers bison flex cmake clang \
    gettext texinfo \
    krb5 krb5-dev \
    libssl1.0 \
    tzdata \
    zlib zlib-dev \
    lttng-ust lttng-ust-dev \
    man man-pages less lzip tree file abuild git docs && \
    ln -sf bash /bin/sh

#ENV TERM=xterm
#ENV PATH=/sbin:/bin:/usr/sbin:/usr/bin
WORKDIR /tmp

#RUN git clone https://github.com/alpinelinux/aports.git

WORKDIR /root

CMD [ "/bin/bash",  "-l" ]
