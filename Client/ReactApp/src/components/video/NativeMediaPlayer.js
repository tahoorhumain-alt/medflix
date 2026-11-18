import React from 'react';

export default function NativeMediaPlayer({ videoSource, visible, onCloseClick }) {
    // If the player is not supposed to be visible, render nothing
    if (!visible) return null;

    return (
        <div style={{ position: 'fixed', top: 0, left: 0, width: '100vw', height: '100vh', backgroundColor: 'black', zIndex: 9999 }}>
            <button 
                onClick={onCloseClick} 
                style={{ position: 'absolute', top: '20px', left: '20px', zIndex: 10000, padding: '10px', background: 'red', color: 'white', border: 'none', borderRadius: '5px', cursor: 'pointer' }}
            >
                Close Player
            </button>
            <video 
                src={videoSource} 
                controls 
                autoPlay 
                style={{ width: '100%', height: '100%', objectFit: 'contain' }} 
            />
        </div>
    );
}
