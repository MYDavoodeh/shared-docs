#!/usr/bin/env python3
"""The first argument to the script is $a$ and the second is $b$.

Example output:
3*2 = 6"""

import sys


def mp(a, b):
    """Recursively calculate a*b"""  # $a \times b$
    if b == 1:
        return a
    return a + mp(a, b - 1)


a = int(sys.argv[1])
# Default value for b = 1
try:
    b = int(sys.argv[2])
except IndexError:
    b = 1
print("{}*{} = {}".format(a, b, mp(a, b)))
