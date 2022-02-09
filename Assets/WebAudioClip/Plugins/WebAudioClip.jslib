
mergeInto(LibraryManager.library, {

  Initcontroller: function() {
    this.audioCtx = new AudioContext();
    this.clips = {};
  },

  CreateClip: function (id, channels) {
    const clip = this.audioCtx.createBufferSource();
    this.clips[id] = clip;
  },

  DisposeClip: function(id) {
    delete(this.clips[id]);
  },

  UploadData: function(id, array, size, offset, channelCount, frequency) {
    const clip = this.clips[id];
    const buffer = this.audioCtx.createBuffer(channelCount, size, frequency);

    const channelData = [];
    const channelDataSize = Math.floor(size / channelCount);

    for (var i = 0; i < channelCount; ++i) {
      const channelSamples = [];

      for (var j = 0; j < channelDataSize; ++j) {
        channelSamples.push(
          HEAPF32[(array >> 2) + ((i * channelDataSize) + j)]
        );
      }

      var bufData = Float32Array.from(channelSamples);
      buffer.copyToChannel(bufData, i, offset)
    }

    clip.buffer = buffer;
  },

  Start: function(id, time) {
    this.audioCtx.resume();

    const clip = this.clips[id];
    const newClip = this.audioCtx.createBufferSource();

    newClip.buffer = clip.buffer;

    var timeFloat = HEAPF32[time >> 2];

    console.log("Start clip from " + timeFloat);
    newClip.start(0, timeFloat);
    newClip.connect(this.audioCtx.destination);

    this.clips[id] = newClip;
  },

  Stop: function(id) {
    this.audioCtx.suspend();

    const clip = this.clips[id];
    console.log("Stop clip!");
    clip.stop();
    clip.disconnect(this.audioCtx.destination);
  }

});
