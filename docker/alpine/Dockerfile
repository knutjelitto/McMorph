FROM alpine:edge

RUN apk update && apk upgrade && apk add --no-cache \
    bash coreutils man man-pages less lzip tree file abuild build-base git docs && \
    ln -sf bash /bin/sh

#ENV TERM=xterm
#ENV PATH=/sbin:/bin:/usr/sbin:/usr/bin
WORKDIR /root

CMD [ "/bin/bash",  "-l" ]
