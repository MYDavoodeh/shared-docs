#!/usr/bin/env python3
"""The first argument to the script is $n$.

Example output:
For n times $\sqrt{6 + \sqrt{6 + \sqrt{6 + ...}}}$ = """

import sys, math


def sqrt(n):
    """Recursively calculate sqrt(6 + sqrt(6 + sqrt(6 + ...))"""  # $\sqrt{6 + \sqrt{6 + \sqrt{6 + \ldots}}}$
    if n == 1:
        return math.sqrt(6)
    return math.sqrt(sqrt(n - 1) + 6)


n = int(sys.argv[1])
print(
    "For {}".format(n),
    r" times sqrt(6 + sqrt(6 + sqrt(6 + ...)) = ",
    "{}".format(sqrt(n)),
)
