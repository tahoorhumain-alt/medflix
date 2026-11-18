import React, { useState } from 'react';

export default function NativeMediaPlayer({ videoSource, visible, onCloseClick }) {
  const [error, setError] = useState(null);

  if (!visible) return null;

  return (
    <div
      style={{
        position: 'fixed',
        top: 0,
        left: 0,
        width: '100vw',
        height: '100vh',
        backgroundColor: 'black',
        zIndex: 9999,
      }}
    >
      <button
        onClick={onCloseClick}
        style={{
          position: 'absolute',
          top: '20px',
          left: '20px',
          zIndex: 10000,
          padding: '10px',
          background: 'red',
          color: 'white',
          border: 'none',
          borderRadius: '5px',
          cursor: 'pointer',
        }}
      >
        Close Player
      </button>

      {error && (
        <div style={{ color: 'white', padding: '60px 20px' }}>
          Video error: {error}
        </div>
      )}

      <video
        key={videoSource}          // forces React to recreate video when source changes
        src={videoSource}
        controls
        autoPlay
        muted                     // required for mobile autoplay
        playsInline               // required for inline playback on iOS
        preload="metadata"
        onError={(e) => {
          const v = e.currentTarget;
          setError(
            v.error
              ? `code ${v.error.code}: ${v.error.message || 'unknown'}`
              : 'unknown video error'
          );
        }}
        style={{
          width: '100%',
          height: '100%',
          objectFit: 'contain',
        }}
      />
    </div>
  );
}
