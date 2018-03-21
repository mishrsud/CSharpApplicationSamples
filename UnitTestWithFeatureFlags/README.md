# Summary
Collate thoughts on how to effectively and efficiently test a system that has feature flags in place

## References
1. Martin Fowler: Feature Toggle
2. Pete Hodgson: Feature Toggle

## Assumptions
- Flags statuses are boolean only - either ON or OFF
- Flag status of OFF implies old behaviour from a system. i.e. as if new implementation wasn't even there
- Flag status of ON implies new behaviour 

## Problem Definition
A system that has feature flags in place needs to be tested
- To ensure the behaviour conforms to expectations when flags are turned OFF (old behaviour)
- To ensure the behaviour conforms to expectations when flags are turned ON  (new behaviour)

## Simplest possible implementation
(pseudo code)
- Call the Flag configuration provider
- Flag status ON?
    - Assert expectations from new implementation
- Flag status OFF?
    - Assert expectations from old implementation
    Pros of this approach:
     - Straightforward to implement
    Cons of this approach:
     - Every test that depends on the status of the feature flag needs to write repetetive conditionals that make one set of assertions or another

## Alternative approach for Tests in .NET
    An attribute, e.g. [IgnoreIf("flag-name", true)]
    
