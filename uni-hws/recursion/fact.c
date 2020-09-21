/* Author: M. Yas. Davoodeh <MYDavoodeh@gmail.com> */
/* https://github.com/MYDavoodeh */
/* Recursively calculate factorial of $n$ */

/* Example output ******************/
/* 3! = 6                          */
/***********************************/

#include <stdio.h>
#include <stdlib.h>

unsigned long long fact(short n);

int main(int argc, char *argv[]) {
  if (argc < 2 || atoi(argv[1]) < 0) {
    printf("ERROR: Bad input! Enter a positive number\n");
    return 1;
  }

  int input = atoi(argv[1]);
  printf("%d! = %llu\n", input, fact(input));
  return 0;
}

unsigned long long fact(short n) {
  if (n == 0 || n == 1)
    return 1;
  return n * fact(n - 1);
}
