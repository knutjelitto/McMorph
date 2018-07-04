echo "sourcing ~/.profile"

if [ "$BASH" ]; then
    if [ -f ~/.bashrc ]; then
        source ~/.bashrc
    fi
fi
