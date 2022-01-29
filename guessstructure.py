import queue
from sys import stdin
from queue import PriorityQueue
from collections import deque

while True:
    try:
        count = int(input())
    except(EOFError):
        break

    stack = []
    queue = deque()
    priority_queue = PriorityQueue()

    possible = ["Queue", "PriorityQueue", "Stack"]

    for i in range(0, count):
        operation = input().split(" ")
        value = int(operation[1])

        # Add
        if operation[0] == "1":
            stack.append(value)
            queue.append(value)
            # Negative value as smallest value is popped first
            priority_queue.put(-value)
        else:
            # Remove (test if the removed item matches)
            if len(stack) == 0 or stack.pop() != value:
                if "Stack" in possible:
                    possible.remove("Stack")
            if len(queue) == 0 or queue.popleft() != value:
                if "Queue" in possible:
                    possible.remove("Queue")
            if priority_queue.qsize() == 0 or priority_queue.get() != -value:
                if "PriorityQueue" in possible:
                    possible.remove("PriorityQueue")

    # Print answer
    if len(possible) == 0:
        print("impossible")
    elif len(possible) == 1:
        if possible[0] == "Queue":
            print("queue")
        elif possible[0] == "PriorityQueue":
            print("priority queue")
        elif possible[0] == "Stack":
            print("stack")
    else:
        print("not sure")
