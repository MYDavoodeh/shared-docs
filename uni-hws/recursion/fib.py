#!/usr/bin/env python3
"""The first argument to the script is $n$.

Example output:
fib(6) = 8"""

import sys


def fib(n):
    """Recursively calculate n-th item in the Fibonacci's series."""
    if n == 1 or n == 2:
        return 1
    else:
        return fib(n - 1) + fib(n - 2)


n = int(sys.argv[1])
print("fib({}) = {}".format(n, fib(n)))
