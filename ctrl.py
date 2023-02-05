import os
import signal
import sys
import time

h = int(input("Enter the value of h: "))

while True:
    pid = os.fork()
    if pid == 0:
        str_h = str(h)
        outfile = open("pddl.out", "w")
        os.dup2(outfile.fileno(), sys.stdout.fileno())
        os.execvp("./ff", ["./ff", "-O", "-o", "domain.pddl", "-f", "problem.pddl", "-h", str_h])
    else:
        signal.signal(signal.SIGALRM, signal.SIG_IGN)
        signal.alarm(6)
        pid, ret = os.waitpid(-1, 0)
        print("Alarm here!")
        signal.alarm(0)
        if os.WIFEXITED(ret):
            break
        else:
            h += 1
