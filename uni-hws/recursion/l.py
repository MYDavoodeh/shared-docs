#!/usr/bin/env python3
n = 25


def l(n):
    if n == 1:
        return 0
    return l(n // 2) + 1


# $l(25) \rightarrow \underbrace{l(12)}_{\underbrace{l(6)}_{l(3)+1}+1}+1
# $l(25) = 4$
print("l({}) = {}".format(n, l(n)))
