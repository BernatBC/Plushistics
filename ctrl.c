#include <unistd.h>
#include <stdio.h>
#include <stdlib.h>
#include <sys/wait.h>

int h, fd;

void timeout (int s)
{
    kill(fd, SIGKILL);
}

int main(int argc, char** argv)
{
    h = atoi(argv[1]);

    while (true){

        fd = fork();
        if (fd == 0){
            char str[2];
            sprintf(str, "%d", h);
            execlp("./ff", "./ff", "-O",  "-o", "domain.pddl", "-f", "problem.pddl", "-h", str, (char * ) NULL);
        } else {
            struct sigaction sa;
            sa.sa_handler = &timeout;
            sa.sa_flags = SA_RESTART;
            sigfillset(&sa.sa_mask);
            sigaction(SIGALRM, &sa, NULL);

            alarm(6);
            
            int ret, pid;

            while ((pid = waitpid(-1, &ret, 0) > 0));


            if (WIFEXITED(ret))
            {
                exit(0);
            }
            else ++h;

        }

    }
}