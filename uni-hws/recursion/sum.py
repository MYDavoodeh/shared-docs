#!/usr/bin/env python3
"""The first argument to the script is n.

Example output:
sum(1, ..., 7) = 28"""

import sys


def fn(n):
    """Recursively calculate 1 + 2 + 3 + ... + n"""
    if n == 0:
        return n
    return n + fn(n - 1)


n = int(sys.argv[1])
print("sum(1, ..., {}) = {}".format(n, fn(n)))
