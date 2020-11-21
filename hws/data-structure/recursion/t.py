#!/usr/bin/env python3
x, y = 5, 2


def t(x, y):
    if x <= y or y == 0:
        return x
    elif y == 1:
        return t(x - 1, y) + 1
    return t(t(y, x), y - 1) + 2


# $t(5,2) \rightarrow \underbrace{t(\overbrace{t(y,x)}^{t(2,5)=2},y-1)}_{t(2,1) \rightarrow t(x-1,1)+1 = 2}+2$

# $t(5,2) = 4$
print("t{} = {}".format((x, y), t(x, y)))
