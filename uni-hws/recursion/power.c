/* Author: M. Yas. Davoodeh <MYDavoodeh@gmail.com> */
/* https://github.com/MYDavoodeh */
/* Recursively calculate $a^b$ */

/* Example output ******************/
/* 3^2 = 9                         */
/***********************************/

#include <stdio.h>
#include <stdlib.h>

unsigned long long mp(int a, int b);

int main(int argc, char *argv[]) {
  if (argc < 2 || atoi(argv[1]) < 0 || atoi(argv[2]) < 0) {
    printf("ERROR: Bad input! Enter two positive integers\n");
    return 1;
  }

  int power;
  if (argc == 2)
    power = 1;
  else
    power = atoi(argv[2]);

  int base = atoi(argv[1]);
  printf("%d^%d = %llu\n", base, power, mp(base, power));
  return 0;
}

unsigned long long mp(int a, int b) {
  if (b == 1)
    return a;
  return a * mp(a, b - 1);
}
