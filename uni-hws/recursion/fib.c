/* Author: M. Yas. Davoodeh <MYDavoodeh@gmail.com> */
/* https://github.com/MYDavoodeh */
/* Recursively calculate $n$th item in the Fibonacci's series */

/* Example output ******************/
/* fib(6) = 8                      */
/***********************************/

#include <stdio.h>
#include <stdlib.h>

unsigned long long fib(int n);

int main(int argc, char *argv[]) {
  if (argc < 2 || atoi(argv[1]) <= 0) {
    printf("ERROR: Bad input! Enter a positive number\n");
    return 1;
  }

  int input = atoi(argv[1]);

  printf("fib(%d) = %llu\n", input, fib(input));
  return 0;
}

unsigned long long fib(int n) {
  if (n == 1 || n == 2)
    return 1;
  return fib(n - 1) + fib(n - 2);
}
