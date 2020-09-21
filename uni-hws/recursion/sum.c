/* Author: M. Yas. Davoodeh <MYDavoodeh@gmail.com> */
/* https://github.com/MYDavoodeh */
/* Recursively calculate $1+2+3+...+n$ */

/* Example output ******************/
/* 28 or:                          */
/*      1 + 2 + 3 + 4 + 5 + 6 + 7  */
/***********************************/

#include <stdio.h>
#include <stdlib.h>

unsigned long long fn(int n);

int main(int argc, char *argv[]) {
  if (argc < 2 || atoi(argv[1]) < 0) {
    printf("ERROR: Bad input! Enter a positive number\n");
    return 1;
  }

  int input = atoi(argv[1]);
  printf("%llu or:\n\t", fn(input));
  for (int i = 1; i < input; i++) {
    printf("%d + ", i);
  }
  printf("%d\n", input);
  return 0;
}

unsigned long long fn(int n) {
  if (n == 0)
    return n;
  return n + fn(n - 1);
}
