#!/usr/bin/env python3
"""The first argument to the script is $n$.

Example output:
3! = 6"""

import sys


def fact(n):
    """Recursively calculate factorial of $n$"""
    if n == 0 or n == 1:
        return 1
    return n * fact(n - 1)


n = int(sys.argv[1])
print("{}! = {}".format(n, fact(n)))
