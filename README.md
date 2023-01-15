# The Problem
Smartphones are often used in conversation in an awkward or intrusive fashion. We’ve all had to huddle around someone’s phone to see that photo or meme video they’re excited to share. AirDrop XR lets you drag-and-drop content (images, videos, and sketches) from your phone to the world for anyone with an Augmented Reality headset to see.

Interfaces and interactions have been fined-tuned for smartphones over the past few decades, but are still in their infancy for head-worn displays. Sharing across mobile devices is supported, but isn’t handsfree and doesn’t utilize 3D space. AirDrop XR leverages both the robust interactions from the mobile phone to find and place content, and the handsfree, spatial experience of augmented reality for content viewing.
Inspiration
As the name of our project suggests, we are inspired by Apple’s AirDrop for its seamless cross-device file transfer functionality. As we move from two dimensions to three, we look for physical objects as inspirations for our interactions and visualizations. The looks and control of the elements resemble a selfie stick, with one end tethered to the smartphone, and the other holding the audiovisual component. 
# The Solution
We created AirDrop XR as a whimsical way to unlock the content trapped on our phones by literally flicking it off the phone into the real world.

Our project enables users to drop files from the phone to the virtual environment to browse. Users can use their smartphone to pinch, zoom, and drag the images and videos in the virtual environment. In a sense, the smartphone works as a remote that controls the attributes of the transferred image. We also implemented a sketch function that allows users to draw on their phone, and then view it in XR.
# How We Built It
We focus on Meta Quest Pro as the main XR platform for its flexibility to build, and high fidelity passthrough feature. Therefore we consider Quest Pro a good prototype for future XR headsets.

Our solution includes two Unity apps, one for the XR headset for the visualization and audio, and the other on mobile devices that manipulate objects and allow users to draw a quick sketch. We use Normcore’s multiplayer capabilities to send commands from the phone to the headset, as well as a Python HTTP server to handle file transfers. We have tested our application with Quest Pro, as well as both iOS and Android smartphones.
Challenges
Having phones and headsets in the same AR environment involves two major multiplayer challenges. 

For one, we need to identify the location of the phones in the virtual space so that we can spawn objects tethered to the device. We implemented two solutions; one using the hand-tracking feature of Quest Pro, and the other using a controller holder to locate the phone. For our demo, we are using the second option for its precision.

In order to send and control objects in the virtual space, we need to also establish connections between the phone and the headset. While Normcore can send control signals between the devices, we also need to have a separate server for sending the audiovisual files. Therefore our solution includes two different networking components.
Accomplishments
Our application allows users to seamlessly transfer files from smartphones to headsets, navigate them with their smartphones, and view them in the 3D space in whichever way they prefer. Users can use our sharing and viewing features in social, educational, professional, and entertainment scenarios. We are incredibly excited to see our toolset being used in homes, offices, classrooms, factories, and beyond.
# What We Learned
Version control + Unity == DANGER

Dillon: Github Collaboration (for Unity)
Daisy: Github Collaboration (for Unity)
Robert: Unity, HTTP Server
Jon: Unity, Normcore, 
Mike: 
What’s Next

# Looking ahead
we hope to get funding through Snapdragon’s Metaverse Fund. We will extend support to more file types, such as 3D models and dynamic web pages. Most importantly, as an open source project, we cannot wait to see the applications other developers can build on top of our toolset.


