### Recursion
**Recursion Problem Set:** *https://codeforces.com/group/MWSDmqGsZm/contest/223339*


### LikedList + Stack + Queue
1. Sorting a linkedlist:
    >[!TIP] 
    > use any sorting algorithm for linkedlist
2. Reverse an singly linkedlist
    >[!TIP]
    > bruteforce solution using temp variables
3. Meging two linkedlist
    >[!TIP]
    > possible two solution:
    > 1. new linkedlist meging two linkedlist
    > 2. modifing the existing linkedlists
4. Finding the middle node
    >[!TIP]
    > slow-fast pointer/hare and tortoise pointer technique
5. Finding cycle
    >[!TIP]
    > possible two solution:
    > 1. keeping new field in the Node struct(bool isVisited). then checking if node is previously visited or not.
    > 2. using slow-fast pointer/hare and tortoise pointer technique

**Practice problem links:**
1. *https://leetcode.com/problems/baseball-game/description/*
2. *https://leetcode.com/problems/crawler-log-folder/description/*
3. *https://leetcode.com/problems/final-prices-with-a-special-discount-in-a-shop/description/*
4. *https://leetcode.com/problems/time-needed-to-buy-tickets/description/*
5. *https://leetcode.com/problems/number-of-students-unable-to-eat-lunch/description/*
6. *https://leetcode.com/problems/removing-stars-from-a-string/description/*
7. *https://leetcode.com/problems/add-two-numbers/description/*


### Binary Tree
1. Build a tree from command line input
    >[!TIP]
    > 1. take string stream then convert it into a vector
    > 2. take a value from vector and push it to a queue
    > 3. check front() and perfrom the operation then pop from queue
    > 4. also push the child nodes into the queue if it is not null

2. DFS traversal
    >[!TIP]
    > 1. recusive call to left subtree and right subtree utill the node is null

5. BFS traversal/ level-order traversal
    >[!TIP]
    > 1. use a queue
    > 2. take front and operate then pop the value from queue and push its child nodes.

**Practice problem links:**
1. *https://leetcode.com/problems/invert-binary-tree*
2. *https://leetcode.com/problems/sum-of-left-leaves*
3. *https://leetcode.com/problems/diameter-of-binary-tree*
4. *https://leetcode.com/problems/binary-tree-tilt*
5. *https://leetcode.com/problems/construct-string-from-binary-tree*
6. *https://leetcode.com/problems/average-of-levels-in-binary-tree*
7. *https://leetcode.com/problems/second-minimum-node-in-a-binary-tree*
8. *https://leetcode.com/problems/leaf-similar-trees*
9. *https://leetcode.com/problems/univalued-binary-tree*
10. *https://leetcode.com/problems/same-tree*
11. *https://leetcode.com/problems/symmetric-tree*
12. *https://leetcode.com/problems/minimum-depth-of-binary-tree*
13. *https://leetcode.com/problems/balanced-binary-tree*