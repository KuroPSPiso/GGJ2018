using System.Collections;
using UnityEngine;

public class Terminal : MonoBehaviour
{
    public Transform GreenParticleContainer;
    public Transform BlueParticleContainer;
    public Transform[] GreenParticlePoints;
    public Transform[] BlueParticlePoints;

    private bool _greenTeamLoading = false;
    private bool _blueTeamLoading = false;

    private float _greenTeamProgress = 0;
    private float _blueTeamProgress = 0;

    private Coroutine _greenTeamLoadingCoroutine;
    private Coroutine _blueTeamLoadingCoroutine;

    public GameObject[] packageUI;
    int[] nPackages = { 0, 0 };

    private void Start()
    {
        if (GreenParticleContainer != null)
        {
            GreenParticleContainer.gameObject.SetActive(false);
        }

        if (BlueParticleContainer != null)
        {
            BlueParticleContainer.gameObject.SetActive(false);
        }
    }

    public void AddPackage(int team)
    {
        packageUI[team * 2 + nPackages[team]].SetActive(false);
        nPackages[team]++;
    }

    public void RemovePackage(int team)
    {
        nPackages[team]--;
        packageUI[team * 2 + nPackages[team]].SetActive(true);
    }

    public bool CanAddPackage(int team)
    {
        return nPackages[team] < 2;
    }

    public void UpdateProgress(int team, float progress)
    {
        if (team == 0)
        {
            _greenTeamProgress = progress;
        }
        else
        {
            _blueTeamProgress = progress;
        }
    }

    public bool CanUpdateTerminal(int team)
    {
        if (team == 0 && _greenTeamLoading)
        {
            return false;
        }
        else if (team == 1 && _blueTeamLoading)
        {
            return false;
        }

        return true;
    }

    public void StartLoading(int team)
    {
        Debug.Log("start loading " + team);

        _greenTeamLoading = team == 0;
        _blueTeamLoading = team == 1;

        if (_greenTeamLoading)
        {
            if (_greenTeamLoadingCoroutine != null)
            {
                StopCoroutine(_greenTeamLoadingCoroutine);
            }

            _greenTeamLoadingCoroutine = StartCoroutine(ChargeTerminal(isGreen: true));
        }
        else if (_blueTeamLoading)
        {
            if (_blueTeamLoadingCoroutine != null)
            {
                StopCoroutine(_blueTeamLoadingCoroutine);
            }

            _blueTeamLoadingCoroutine = StartCoroutine(ChargeTerminal(isGreen: false));
        }
    }

    public void StopLoading(int team)
    {
        Debug.Log("stop loading " + team);
        if (team == 0)
        {
            StopCoroutine(_greenTeamLoadingCoroutine);
            _greenTeamLoading = false;
            _greenTeamProgress = 0;
            GreenParticleContainer.position = GreenParticlePoints[0].position;
            GreenParticleContainer.gameObject.SetActive(false);
        }
        else
        {
            StopCoroutine(_blueTeamLoadingCoroutine);
            _blueTeamLoading = false;
            _blueTeamProgress = 0;
            BlueParticleContainer.position = BlueParticlePoints[0].position;
            BlueParticleContainer.gameObject.SetActive(false);
        }
    }

    public void FinishLoading(int team)
    {
        Debug.Log("finish loading " + team);
        if (team == 0)
        {
            StopCoroutine(_greenTeamLoadingCoroutine);
            _greenTeamLoading = false;
            GreenParticleContainer.position = GreenParticlePoints[0].position;
            _greenTeamProgress = 0;
        }
        else
        {
            StopCoroutine(_blueTeamLoadingCoroutine);
            _blueTeamLoading = false;
            BlueParticleContainer.position = BlueParticlePoints[0].position;
            _blueTeamProgress = 0;
        }
    }

    private IEnumerator ChargeTerminal(bool isGreen)
    {
        yield return null;

        var progress = isGreen ? _greenTeamProgress : _blueTeamProgress;
        var particleContainer = isGreen ? GreenParticleContainer : BlueParticleContainer;
        var particlePoints = isGreen ? GreenParticlePoints : BlueParticlePoints;

        particleContainer.gameObject.SetActive(true);

        while (progress < 0.5f)
        {
            progress = isGreen ? _greenTeamProgress : _blueTeamProgress;
            //Move towards second point, from first.
            particleContainer.position = Vector3.Lerp(particlePoints[0].position, particlePoints[1].position, (progress * 2f));
            yield return null;
        }

        while (progress < 1)
        {
            progress = isGreen ? _greenTeamProgress : _blueTeamProgress;
            //Move towards third point, from second.
            particleContainer.position = Vector3.Lerp(particlePoints[1].position, particlePoints[2].position, (progress - 0.5f) * 2f);
            yield return null;
        }
    }
}
