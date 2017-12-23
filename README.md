# Gambler's Sampler

The "gambler's fallacy" is explained by Wikipedia (as of December 23rd, 2017) as follows:

> The **gambler's fallacy**, also known as the **Monte Carlo fallacy** or the **fallacy of the maturity of chances**, is the belief that, if something happens more frequently than normal during some period, it will happen less frequently in the future, or that, if something happens less frequently than normal during some period, it will happen more frequently in the future (presumably as a means of balancing nature). In situations where what is being observed is truly random (i.e., independent trials of a random process), this belief, though appealing to the human mind, is false.

Gamers using real dice and other independent chance devices, of course, have to accept and work with this fact. 
Uniformly distributed random numbers are also the best for cryptography, 
since the bits of entropy for the random numbers are not fully utilized under any other scheme.

But we're programmers. We can fit an entire game of Blackjack onto a chip smaller than a single dice. 
Surely we can do better than this.

A JavaScript module released in July 2017, [xori/gamblers-dice](https://github.com/xori/gamblers-dice),
provides a simple die with these properties. 
The module was [shared on Y-Combinator](https://news.ycombinator.com/item?id=14805265), 
with a string of comments discussing possible applications and related mathematical distributions, 
such as the [Pólya urn](https://en.wikipedia.org/wiki/P%C3%B3lya_urn_model) model.

This module is a successor to that module written in the C# programming language, with the following main features:

- Any set of outcomes can be returned, and can be weighted arbitrarily (in any proportion!).
- Sampling is, on average, faster than time linear to the number of outcomes.
- The amount by which the Gambler's Fallacy takes effect can be tweaked arbitrarily via a distortion parameter.
- The state of the generator can be exported and serialized at any time.

# How to Use This Module

This module can be used both as a .NET DLL and as source. It can be imported into any C# project, 
including both .NET 4 projects, Mono projects, and Unity projects.

Once you have the module imported, either use one of the built-in factory methods to create a sampler:
```
using Betafreak.GamblersSampler;
ISampler uniform_d6 = Samplers.UniformD6();
ISampler gamblers_d20 = Samplers.GamblersD20();
```

Or, create your own with a custom set of outcomes:
```
using Betafreak.GamblersSampler;
ISampler gamblers_name_picker = new GamblersSampler<string>(
    new string[] { "Alice", "Bob", "Chuck" },
    0.75);
ISampler weighted_name_picker = new GamblersSampler<string>(
    new WeightedPair<string>[] {
       new WeightedItem<string>( "Alice", 2.1 ),
       new WeightedItem<string>( "Bob", 1.0 ),
       new WeightedItem<string>( "Chuck", Math.PI )
    },
    0.8);
```

Once you have the sampler, simply sample it when you need a random outcome:
```
int shot_value = gamblers_d20.Next();
if (shot_value > shot_chance * 100) {
    // shot hits
} else {
    // shot misses
}

for (int i = 0; i < 10; i++) {
    Console.Writeline("Person {0}) {1}", i, weighted_name_picker.Next());
}
```

# Acknowledgments

- Inspired by Xori's [gamblers-dice](https://github.com/xori/gamblers-dice) module.
