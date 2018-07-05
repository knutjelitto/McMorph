FROM voidlinux/voidlinux-musl

ENV TERM=xterm

RUN xbps-install --sync --update --yes && \
    xbps-install --sync --update --yes && \
    xbps-install --yes git && \
    (cd /tmp; git clone https://github.com/void-linux/void-packages.git; cd void-packages; ./xbps-src binary-bootstrap)

RUN xbps-install --yes base-devel tree nano

RUN xbps-alternatives --set bash && \
    rm -rv /var/cache/xbps/* && \
    mv /etc/profile.d/colorls.sh /etc/profile.d/colorls.sh-disabled && \
    # remove non-docker content
    rm -rv /boot /home /media /mnt /opt /run

CMD [ "/bin/bash",  "-l" ]
