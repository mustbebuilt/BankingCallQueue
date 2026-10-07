# Online Banking Call Queue Simulation (Circular Queue)

This project implements a **Circular Queue** in C# to simulate a call queue system for a helpdesk at **FinTech Bank**, as part of **Algorithms and Data Structures (Week 3 - Session 2)**.

---

## 1. Scenario Overview

- **Context**: FinTech Bank operates a customer support helpdesk where incoming customer calls are queued until the receptionist (**Sarah**) is available to answer them.
- **Queue Capacity**: The telephone system holds a **maximum of 6 callers** at any one time (`maxSize = 6`).
- **Call Handling**:
  - Sarah handles callers strictly **one-by-one** in **First-In, First-Out (FIFO)** order.
  - If the queue is **full**, incoming callers receive a busy signal / rejection message and cannot join.
  - If the queue is **empty** when Sarah tries to answer a call, the system reports that Sarah is idle.

---

## 2. Why a Circular Queue?

In a standard linear array-based queue, dequeuing elements leaves unused space at the beginning of the array unless all elements are shifted forward (an $O(N)$ operation). 

A **Circular Queue** solves this by conceptually connecting the end of the array back to the beginning:
- **Fixed Size ($O(1)$ Memory)**: Bounded to 6 slots, preventing unbounded memory growth and system overload.
- **$O(1)$ Time Complexity**: Enqueue and Dequeue operations operate in constant time without shifting elements.
- **Slot Reusability**: Reuses memory slots vacated by served callers via modulo arithmetic:
  $$\text{rear} = (\text{rear} + 1) \pmod{\text{maxSize}}$$
  $$\text{front} = (\text{front} + 1) \pmod{\text{maxSize}}$$

---

## 3. Data Structure Components & Operations

### Components
- `queue[]`: Array of strings (size 6) storing caller IDs.
- `front`: Index of the next caller to be served (initially `-1`).
- `rear`: Index of the most recently enqueued caller (initially `-1`).
- `size`: Current count of callers in the queue (initially `0`).
- `maxSize`: Maximum capacity (`6`).

### Core Operations
- `enqueue(callerId)`: Adds a caller to the rear if not full; otherwise rejects the call.
- `dequeue()`: Connects Sarah to the caller at `front` and advances `front` if not empty; otherwise reports idle.
- `isFull()`: Returns `true` if `size == maxSize`.
- `isEmpty()`: Returns `true` if `size == 0`.
- `print()`: Displays active callers from `front` to `rear`.

---

## 4. Task 1: Step-by-Step Simulation Walkthrough

Below is the complete state table for the simulation sequence. 

> **Note**: Events **1–17** represent the worksheet exercise from **Task 1** of the PDF. Events **18–20** were added in [`Program.cs`](file:///Users/martincooper/Documents/learning-experiments/algorithms-data-structures-module/WeekThree/BankingCallQueue/Program.cs) to demonstrate draining the queue and testing the empty/underflow condition when Sarah becomes idle.

- **Initial State**: `front = -1`, `rear = -1`, `size = 0`, `queue = [ -, -, -, -, -, - ]`

| # | Event | Array State `[0..5]` | Front | Rear | Size | Action / Output Message | Active Queue (`print()`) |
|---|---|:---:|:---:|:---:|:---:|---|---|
| **1** | `C Alice` | `[ Alice, -, -, -, -, - ]` | 0 | 0 | 1 | `Alice added to the Queue` | `Alice` |
| **2** | `C Bob` | `[ Alice, Bob, -, -, -, - ]` | 0 | 1 | 2 | `Bob added to the Queue` | `Alice Bob` |
| **3** | `S` | `[ -, Bob, -, -, -, - ]` | 1 | 1 | 1 | `Connecting to Alice` | `Bob` |
| **4** | `C Charlie` | `[ -, Bob, Charlie, -, -, - ]` | 1 | 2 | 2 | `Charlie added to the Queue` | `Bob Charlie` |
| **5** | `C David` | `[ -, Bob, Charlie, David, -, - ]` | 1 | 3 | 3 | `David added to the Queue` | `Bob Charlie David` |
| **6** | `C Eve` | `[ -, Bob, Charlie, David, Eve, - ]` | 1 | 4 | 4 | `Eve added to the Queue` | `Bob Charlie David Eve` |
| **7** | `S` | `[ -, -, Charlie, David, Eve, - ]` | 2 | 4 | 3 | `Connecting to Bob` | `Charlie David Eve` |
| **8** | `C Frank` | `[ -, -, Charlie, David, Eve, Frank ]` | 2 | 5 | 4 | `Frank added to the Queue` | `Charlie David Eve Frank` |
| **9** | `C Grace` | `[ Grace, -, Charlie, David, Eve, Frank ]` | 2 | 0 | 5 | `Grace added to the Queue` *(Rear wraps to 0)* | `Charlie David Eve Frank Grace` |
| **10** | `C Henry` | `[ Grace, Henry, Charlie, David, Eve, Frank ]` | 2 | 1 | 6 | `Henry added to the Queue` *(Queue is full)* | `Charlie David Eve Frank Grace Henry` |
| **11** | `C Isabella` | `[ Grace, Henry, Charlie, David, Eve, Frank ]` | 2 | 1 | 6 | `Caller Isabella is rejected. Queue is Full` *(No state change)* | `Charlie David Eve Frank Grace Henry` |
| **12** | `S` | `[ Grace, Henry, -, David, Eve, Frank ]` | 3 | 1 | 5 | `Connecting to Charlie` | `David Eve Frank Grace Henry` |
| **13** | `S` | `[ Grace, Henry, -, -, Eve, Frank ]` | 4 | 1 | 4 | `Connecting to David` | `Eve Frank Grace Henry` |
| **14** | `C Jack` | `[ Grace, Henry, Jack, -, Eve, Frank ]` | 4 | 2 | 5 | `Jack added to the Queue` *(Reuses slot 2)* | `Eve Frank Grace Henry Jack` |
| **15** | `S` | `[ Grace, Henry, Jack, -, -, Frank ]` | 5 | 2 | 4 | `Connecting to Eve` | `Frank Grace Henry Jack` |
| **16** | `S` | `[ Grace, Henry, Jack, -, -, - ]` | 0 | 2 | 3 | `Connecting to Frank` *(Front wraps to 0)* | `Grace Henry Jack` |
| **17** | `S` | `[ -, Henry, Jack, -, -, - ]` | 1 | 2 | 2 | `Connecting to Grace` | `Henry Jack` |
| **18** | `S` | `[ -, -, Jack, -, -, - ]` | 2 | 2 | 1 | `Connecting to Henry` | `Jack` |
| **19** | `S` | `[ -, -, -, -, -, - ]` | 3 | 2 | 0 | `Connecting to Jack` | `Queue is Empty!` |
| **20** | `S` | `[ -, -, -, -, -, - ]` | 3 | 2 | 0 | `No callers in the queue. Sarah is idle.` | `Queue is Empty!` |

---

## 5. Task 2: C# Implementation Details

The solution is split into two primary files:

1. **[`BankingCircularQueue.cs`](file:///Users/martincooper/Documents/learning-experiments/algorithms-data-structures-module/WeekThree/BankingCallQueue/BankingCircularQueue.cs)**:
   - Contains the `BankingCircularQueue` class.
   - Manages array indexing, bounds checking, wrap-around logic, and formatted queue printing.
   - Handles edge cases:
     - **Overflow (Queue Full)**: Prints rejection message and does not alter queue.
     - **Underflow (Queue Empty)**: Prints idle receptionist message.

2. **[`Program.cs`](file:///Users/martincooper/Documents/learning-experiments/algorithms-data-structures-module/WeekThree/BankingCallQueue/Program.cs)**:
   - Instantiates `BankingCircularQueue(6)`.
   - Iterates through the list of incoming call events (`C <Name>`) and serve events (`S`), displaying queue state after each operation.

---

## 6. How to Build and Run

Ensure [.NET SDK](https://dotnet.microsoft.com/) is installed on your machine.

### Build
```bash
dotnet build BankingCallQueue.csproj
```

### Run
```bash
dotnet run
```

---

## 7. Sample Simulation Output

```text
Hello, Welcome to FinTech Bank Online Call Service..
*** Online Banking Call System Simulation ***

Alice added to the Queue
Call Queue: 
Alice 
Bob added to the Queue
Call Queue: 
Alice Bob 
Connecting to Alice
Call Queue: 
Bob 
Charlie added to the Queue
Call Queue: 
Bob Charlie 
David added to the Queue
Call Queue: 
Bob Charlie David 
Eve added to the Queue
Call Queue: 
Bob Charlie David Eve 
Connecting to Bob
Call Queue: 
Charlie David Eve 
Frank added to the Queue
Call Queue: 
Charlie David Eve Frank 
Grace added to the Queue
Call Queue: 
Charlie David Eve Frank Grace 
Henry added to the Queue
Call Queue: 
Charlie David Eve Frank Grace Henry 
Caller Isabella is rejected. Queue is Full
Call Queue: 
Charlie David Eve Frank Grace Henry 
Connecting to Charlie
Call Queue: 
David Eve Frank Grace Henry 
Connecting to David
Call Queue: 
Eve Frank Grace Henry 
Jack added to the Queue
Call Queue: 
Eve Frank Grace Henry Jack 
Connecting to Eve
Call Queue: 
Frank Grace Henry Jack 
Connecting to Frank
Call Queue: 
Grace Henry Jack 
Connecting to Grace
Call Queue: 
Henry Jack 
Connecting to Henry
Call Queue: 
Jack 
Connecting to Jack
Call Queue: 
Queue is Empty!
No callers in the queue. Sarah is idle.
Call Queue: 
Queue is Empty!
```
